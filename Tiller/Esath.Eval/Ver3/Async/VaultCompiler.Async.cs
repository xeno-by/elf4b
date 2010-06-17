using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DataVault.Core.Api;
using DataVault.Core.Api.Events;
using DataVault.Core.Helpers;
using System.Linq;
using Esath.Eval.Ver3.Async;
using Esath.Eval.Ver3.Core;
using Esath.Eval.Ver3.Exceptions;
using Esath.Eval.Ver3.Helpers;
using Esath.Pie.Helpers;

namespace Esath.Eval.Ver3
{
    public partial class VaultCompiler
    {
        // protects system variables, so NEVER EVER lock this for a long amount of time
        internal Object _barrier = new Object();

        // a single thread that does all the compilation
        // compiler never spawns multiple threads even if the vault gets changed
        // in the latter case worker detects the changes by itself and terminates prematurely
        // and after that gets restarted by a special waiter thread that always accompanies it
        internal Thread _worker = null;

        // to avoid name collisions (because several workers can be spawned for the same revision of the vault)
        // we assign a unique id to each worker. this also eases finding base classes for overriding
        internal int _nextWorkerSeq = 0;
        internal int _lastSuccessfulWorkerSeq = -1;

        // signals that the worker thread has finished working
        // n0te that this doesn't mean that the result is ready:
        // worker might had been self-terminated because of vault changes
        internal ManualResetEvent _flag = new ManualResetEvent(true);

        // indicates that the compiler has faced an expected exception but can be restarted later
        // later => when any changes to the vault will have been made (possibly, fixing the cause of the exception)
        // edit. any exception (expected or not) thrown in cumulative mode only makes compiler temporarily broken
        // because the menace of memory pollution is almost non-existent for repeated cumulative compilations
        // edit2. _brokenChangeSet tracks the changeset that led to the crash, so that the waiter
        // doesn't infinitely trigger GetCompiledAsync which again crashes and so on
        internal bool _isBroken = false;
        internal ElementChangedEventArgs[] _brokenChangeSet = null;

        // if an expected exception is thrown in-the-middle of the baseline compilation, 
        // then restarting the compilation it poses a significant threat since 
        // retrying full compilation again and again might severely pollute memory of the domain
        // thus, we introduce a threshold of max times a baseline compilation might fail with an expected exception
        internal const int MaxBaselineCompilationFailures = 3;
        internal int _baselineCompilationFailures = 0;

        // indicates that the compiler has faced a severe exception and cannot be used any more
        // this flag cannot be repaired once shit happened
        // 1) if an exception is thrown during codegen initialization (e.g. insufficient rights)
        // 2) if Nth in a row baseline compilation had crashed with any exception (both expected and unexpected count)
        internal bool _isPermanentlyBroken = false;

        // indicates that the we still haven't compiled the initial version of the elf->il assembly
        // this can be caused by insufficient time passed after the constructor was executed or
        // by constant changes of the vault that make worker terminate in the middle of its job
        internal bool _compilingBaseline = true;

        // accumulates significant changes to the vault
        // this list is regularly checked by worker and if it witnesses a new entry, 
        // it gets self-terminated and restarts to embrace the very latest changes
        internal List<ElementChangedEventArgs> _changeSet = new List<ElementChangedEventArgs>();

        // hold several precached instances of latest successfully compiled scenario types
        // once compiler detects significant vault changes, it invalidates this cache and restarts the worker
        internal CompiledScenarioCache _lastResult = null;

        public CompiledScenarioCache GetCompiledSync()
        {
            MTLog.Say("Waiting at the barrier");
            lock (_barrier)
            {
                MTLog.Say("Breached the barrier");
                MTLog.Say(String.Format("Entering compile sync: " +
                    "_worker={0}, _lastSeq={1}, _isBroken={2}, {3}, _compilingBaseline={4} ({5} failures), _changeset=[{6}]{7}",
                    _worker == null ? "null" : _worker.Name,
                    _lastSuccessfulWorkerSeq,
                    _isBroken,
                    _isPermanentlyBroken,
                    _compilingBaseline,
                    _baselineCompilationFailures,
                    _changeSet.Count,
                    _changeSet.Count == 0 ? String.Empty : Environment.NewLine +
                        _changeSet.Select(e => String.Format("{0}: {1}", e.Reason, e.Subject)).StringJoin(Environment.NewLine)
                    ));

                // if the compiler is in broken state, i.e. an unexpected exception has occurred somewhen
                // then we seize all activities and just fall back to the eval2 scheme
                // broken state is unfixable till the next application restart in current implementation
                if (_isPermanentlyBroken)
                {
                    MTLog.Say("Leaving compile sync (reason: permanently broken)");
                    return null;
                }
                else
                {
                    // if the compiler was broken by an excepted exception, then its relatively safe
                    // to attempt recompilation if anything changes later (e.g. user potentially corrects a mistake)
                    if (_isBroken && _brokenChangeSet.SequenceEqual(_changeSet))
                    {
                        MTLog.Say("Leaving compile sync (reason: broken and no pending changes)");
                        return null;
                    }
                    else
                    {
                        // if we've already got some result, and worker thread ain't spawned yet
                        // then we can conclude that previous result 100% corresponds to the current state of the vault
                        // or else the compiler would rerun itself as a response to any significant change
                        // n0te that neither of the vars checked below can be modified from outside since we own the barrier
                        if (!_compilingBaseline && _changeSet.IsEmpty() && _lastResult != null)
                        {
                            MTLog.Say("Leaving compile sync (reason: no need to calc, reusing last result)");
                            return _lastResult;
                        }
                        else
                        {
                            if (_worker == null)
                            {
                                _flag.Reset();

                                var ctx = new CompilationContext(this);
#if FORCE_SINGLE_THREADED
                                WorkerLogic(ctx);
#else
                                _worker = new Thread(() => WorkerLogic(ctx));
                                _worker.Name = "worker_" + (ctx.CompilingBaseline ? "base" : "cumu") +
                                    ctx.Revision.ToString("000") + "_seq" + ctx.WorkerSeq.ToString("000") + GetThreadStack();
                                MTLog.Say(String.Format("Spawning the worker thread: {0}", _worker.Name));
                                _worker.Start();
#endif

                                // here we don't return: scroll down to see the rest of the code
                            }
                        }
                        
                    }
                }
            }

            // wait until the worker thread is finished (btw the worker can be killed 
            // and restarted if any changes in the vault occur)
            // this means that theoretically a blocking call to GetCompiledSync 
            // might cause caller to freeze for indefinite amount of time

            // to prevent freezing caller should expose the Vault (effectively prohibiting writes) 
            // before a synchronous call to compile

            // important: the wait should be performed outside the lock (_barrier){}
            // or else the worker wouldn't be able to complete execution
            MTLog.Say("Waiting for the _flag event");
            _flag.WaitOne();
            MTLog.Say("_flag event in signal state");

            MTLog.Say("Reentering compile sync (reason: worker has just finished)");

            // here we shouldn't just return the lastResult, since by this moment it could already be changed
            // so just enter the recursion (sigh, i wish i could force compiler to emit the "tailcall" instruction)
            return GetCompiledSync();
        }

        public CompiledScenarioCache GetCompiledAsync()
        {
#if FORCE_SINGLE_THREADED
            return GetCompiledSync();
#endif

            // this method is 99% the same as the above one, so reference the latter's comments for more details

            MTLog.Say("Waiting at the barrier");
            lock (_barrier)
            {
                MTLog.Say("Breached the barrier");
                MTLog.Say(String.Format("Entering compile sync: " +
                    "_worker={0}, _lastSeq={1}, _isBroken={2}, {3}, _compilingBaseline={4} ({5} failures), _changeset=[{6}]{7}",
                    _worker == null ? "null" : _worker.Name,
                    _lastSuccessfulWorkerSeq,
                    _isBroken,
                    _isPermanentlyBroken,
                    _compilingBaseline,
                    _baselineCompilationFailures,
                    _changeSet.Count,
                    _changeSet.Count == 0 ? String.Empty : Environment.NewLine +
                        _changeSet.Select(e => String.Format("{0}: {1}", e.Reason, e.Subject)).StringJoin(Environment.NewLine)
                    ));

                if (_isPermanentlyBroken)
                {
                    MTLog.Say("Leaving compile async (reason: broken permanently)");
                    return null;
                }
                else
                {
                    if (_isBroken && _brokenChangeSet.SequenceEqual(_changeSet))
                    {
                        MTLog.Say("Leaving compile async (reason: broken and no pending changes)");
                        return null;
                    }
                    else
                    {
                        if (!_compilingBaseline && _changeSet.IsEmpty() && _lastResult != null)
                        {
                            MTLog.Say("Leaving compile sync (reason: no need to calc, reusing last result)");
                            return _lastResult;
                        }
                        else
                        {
                            if (_worker != null)
                            {
                                // if we currently have a worker thread running, no need to spawn a waiter thread
                                // because that was already done by a thread that had spawned the worker
                                MTLog.Say("Leaving compile async (reason: worker is alive, nothing more to do here)");
                                return null;
                            }
                            else
                            {
                                _flag.Reset();

                                var ctx = new CompilationContext(this);
                                _worker = new Thread(() => WorkerLogic(ctx));

                                _worker.Name = "worker_" + (ctx.CompilingBaseline ? "base" : "cumu") +
                                    ctx.Revision.ToString("000") + "_seq" + ctx.WorkerSeq.ToString("000") + GetThreadStack();
                                MTLog.Say(String.Format("Spawning the worker thread: {0}", _worker.Name));
                                _worker.Start();

                                // here we don't return: scroll down to see the rest of the code
                            }
                        }
                    }
                }
            }

            // the construct below ensures that if worker gets interrupted due to vault modifications
            // then it gets restarted again and again until generation succeeds (potential memory sink!)
            // it also serves another goal: if the worker has just finished calculating and 
            // while it was busy, compiler updated _changeSet, this quantum thread will restart the worker
            var waiter = new Thread(() =>
            {
                MTLog.Say("Entering the waiter thread");

                MTLog.Say("Waiting for the _flag event");
                _flag.WaitOne();
                MTLog.Say("_flag event in signal state");

                GetCompiledAsync();
                MTLog.Say("Leaving the waiter thread");
            });

            waiter.Name = "waiter" + NextWaiterId.ToString("000") + GetThreadStack();
            MTLog.Say("Spawning the waiter thread: " + waiter.Name);
            waiter.Start();

            // unlike the previous method, an asynchronous call doesn't wait for completion of the worker
            // it just returns null (e.g. in that case EvalSession ver3 just falls back to slower, but instantly accessible ver2)
            MTLog.Say("Leaving compile async (reason: spawned worker and its waiter, now leaving)");
            return null;
        }

        private void WorkerLogic(CompilationContext ctx)
        {
#if TRACE
            var s_workingSet = 0L;
            var s_typesInDynamicAssembly = -1;
            var s_time = TimeSpan.Zero;
#endif

            try
            {
#if TRACE
                GC.Collect();
                s_workingSet = Process.GetCurrentProcess().WorkingSet64;
                s_typesInDynamicAssembly = _types.Count();
                s_time = Process.GetCurrentProcess().TotalProcessorTime;

                var firstLine = String.Format(
                    "Entering the worker thread: rev={0} ({1} mode)", ctx.Revision,
                     ctx.CompilingBaseline ? "baseline" : "cumulative");
                var secondLine = String.Format(
                    "The context is: revision={0}, seq={1}, lastSeq={2}",
                    ctx.Revision, ctx.WorkerSeq, ctx.LastSuccessfulWorkerSeq);
                var thirdLine = "Compilation mode: " + (ctx.CompilingBaseline ? "baseline" : "cumulative");
                var fourthLine = String.Format(
                    "The changeset is: [{0}]{1}",
                    ctx.Changeset.Length,
                    ctx.Changeset.Length == 0 ? String.Empty : Environment.NewLine +
                        ctx.Changeset.Select(e => String.Format("  *{0}: {1}", e.Reason, e.Subject)).StringJoin(Environment.NewLine));
                var fifthLine = String.Format(
                    "The normalized changeset is: [{0}]{1}",
                    ctx.NormalizedChangeSet.Count,
                    ctx.NormalizedChangeSet.Count == 0 ? String.Empty : Environment.NewLine +
                        ctx.NormalizedChangeSet.Select(kvp => String.Format("  *{0}: {1}", kvp.Value, kvp.Key)).StringJoin(Environment.NewLine));
                var finalMessage = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", 
                    Environment.NewLine, firstLine, secondLine, thirdLine, fourthLine, fifthLine);
                MTLog.Say(finalMessage);
#endif

                // was used to test empty thread footprint: ~64k
//                var t_scenario = _asm.GetTypes().FirstOrDefault(t => typeof(ICompiledScenario).IsAssignableFrom(t));
//                t_scenario = t_scenario ?? CompileScenarioRecursively(ctx);

                var t_scenario = CompileScenarioRecursively(ctx);
                var pool = new CompiledScenarioCache(Vault, t_scenario);

                MTLog.Say("Waiting at the barrier, pending successfully completed");
                lock (_barrier)
                {
                    // final modification guard (btw, the compiler logic also has such guards: for every node being compiled)
                    // if we fail here during baseline compilation - sigh... so much memory is gonna to be wasted
                    if (!ctx.StillInTouch()) throw new CompilerOutOfTouchException();

                    MTLog.Say("Breached the barrier");
                    _isBroken = false;
                    _brokenChangeSet = null;
                    _isPermanentlyBroken = false;
                    _compilingBaseline = false;
                    _changeSet.Clear();
                    _lastResult = pool;
                    _lastSuccessfulWorkerSeq = ctx.WorkerSeq;
                    MTLog.Say("Successfully completed");
                }
            }
            catch (Exception ex)
            {
                MTLog.Say("Waiting at the barrier, pending error: " + ex);
                lock (_barrier)
                {
                    MTLog.Say("Breached the barrier");
                    _isBroken = true;
                    _brokenChangeSet = ctx.Changeset;
                    if (ctx.CompilingBaseline) _baselineCompilationFailures++;
                    _isPermanentlyBroken = _baselineCompilationFailures >= MaxBaselineCompilationFailures;
                    // don't touch _compilingBaseline -> it should remain unchanged
                    // don't touch _changeSet -> it should remain unchanged
                    _lastResult = null;
                    MTLog.Say("Error: " + ex);
                }
            }
            finally
            {
                try
                {
                    GC.Collect();

#if TRACE
                    var f_WorkingSet = Process.GetCurrentProcess().WorkingSet64;
                    var f_typesInDynamicAssembly = _types.Count();
                    var f_time = Process.GetCurrentProcess().TotalProcessorTime;

                    var memDelta = (f_WorkingSet - s_workingSet) / 1024;
                    var typesDelta = f_typesInDynamicAssembly - s_typesInDynamicAssembly;
                    var timeDelta_sec = (f_time - s_time).Seconds;
                    var timeDelta_ms = (f_time - s_time).Milliseconds;

                    MTLog.Say(String.Format("Leaving the worker thread.{0}"+
                        "Time delta: {1}.{2} sec, memory delta: {3} Kb ({4} Kb total), types generated: {5}, avg memory per type: {6} Kb",
                        Environment.NewLine,
                        timeDelta_sec, 
                        timeDelta_ms,
                        s_workingSet == 0 ? "??" : memDelta.ToString(),
                        f_WorkingSet,
                        s_typesInDynamicAssembly == -1 ? "??" : typesDelta.ToString(),
                        (s_typesInDynamicAssembly == -1 || s_workingSet == 0 || typesDelta == 0) ? "??" : (memDelta / typesDelta).ToString()));
#else
                    MTLog.Say("Leaving the worker thread");
#endif
                }
                finally
                {
                    _worker = null;
                    _flag.Set();
                }
            }
        }

        private bool SignificantChangeFilter(ElementChangedEventArgs e)
        {
            // we CARE about:
            // 1. deleted svd/fla (maybe indirectly via recursion)
            // 2. deleted node (maybe indirectly via recursion)
            // 3. added svd/fla
            // 4. added node
            // 5. changed fla, i.e. "elfCode" value of a "...\_formulaDeclarations\*\" branch got its content changed

            // we DON'T CARE about:
            // anything else

            // special cases of NOT CARING:
            // 1. renamed anything
            //    let's ignore renaming so far, since it's unusual for this app to rename values that have system names
            // 2. changed svd's external link, i.e. "repositoryValue" value of a "...\_sourceValueDeclarations\*\" branch got its content changed
            //    we don't care about the latter since repoValue never gets burned into assembly, but rather gets cached
            //    whereas cached values get cleared anyways on the compiledscenario instance being reused

            // any branch event is important, since both nodes and svds/flae are branches
            if (e.Subject is IBranch)
            {
                // a special case we process here
                // imagine that a "_sourceValueDeclarations" node is being added to just create scenario node
                // this event in itself is totally irrelevant to recompilation, since it brings nothing new to the table
                // however, when that node gains any children - then we surely proceed to recompile the assembly

                var b = (IBranch)e.Subject;
                if (e.Reason == EventReason.Add && (b.IsFormulaHost() || b.IsSvdHost()))
                {
                    return false;
                }

                if (e.Reason == EventReason.Add || e.Reason == EventReason.Remove)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var value = (IValue)e.Subject;
                if (value.Name == "elfCode")
                {
                    if (e.Reason == EventReason.Content)
                    {
                        var oldContent = ((Stream)e.OldValue).AsString();
                        var newContent = ((Stream)e.NewValue).AsString();
                        return oldContent != newContent;
                    }
                    else if (e.Reason == EventReason.Remove)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        private void SignificantChangeProcessor(ElementChangedEventArgs e)
        {
            lock (_barrier)
            {
                _changeSet.Add(e);
                GetCompiledAsync();
            }
        }

        private object _nextWaiterIdLock = new object();
        private int _nextWaiterId = 0;

        public int NextWaiterId
        {
            get
            {
                lock (_nextWaiterIdLock)
                {
                    return _nextWaiterId++;
                }
            }
        }

        private String GetThreadStack()
        {
            var threadstack = Thread.CurrentThread.Name ?? String.Empty;
            if (threadstack.Contains("_stack="))
            {
                var iof = threadstack.IndexOf("_stack=");
                var myName = threadstack.Substring(0, iof);
                threadstack = threadstack.Substring(iof + "_stack=".Length);
                threadstack += (", " + myName);
            }
            else
            {
                threadstack = "null";
            }

            return "_stack=" + threadstack;
        }
    }
}