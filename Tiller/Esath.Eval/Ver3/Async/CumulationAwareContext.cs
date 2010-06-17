using System;
using System.Collections.Generic;
using System.Linq;
using DataVault.Core.Api;
using DataVault.Core.Api.Events;
using DataVault.Core.Helpers;
using Esath.Eval.Ver3.Core;
using Esath.Eval.Ver3.Helpers;
using Esath.Eval.Ver3.Snippets;
using Esath.Pie.Helpers;

namespace Esath.Eval.Ver3.Async
{
	using DataVault.Core.Helpers.Assertions;
	using DataVault.Core.Helpers.Reflection;
	using DataVault.Core.Helpers.Reflection.Shortcuts;

	public class CumulationAwareContext : SynchronizationContext
    {
        public ElementChangedEventArgs[] Changeset { get; private set; }
        public bool CompilingBaseline { get; private set; }
        public bool CumulativeCompilation { get { return !CompilingBaseline; } }

        protected CumulationAwareContext(VaultCompiler compiler)
            : base(compiler)
        {
            lock (compiler._barrier)
            {
                Changeset = compiler._changeSet.ToArray();
                CompilingBaseline = compiler._compilingBaseline;

                if (CompilingBaseline)
                {
                    // until we've compiled baseline, we're always instantly embracing all the changes
                    compiler._changeSet.Clear();
                    Changeset = new ElementChangedEventArgs[0];

                    // however, when the baseline is ready and we switch to cumulative mode, then
                    // changes should be deleted only when the worker has successfully incorporated them
                }
            }
        }

        public bool StillInTouch()
        {
            lock (Compiler._barrier)
            {
                (CompilingBaseline == Compiler._compilingBaseline).AssertTrue();
                // no need to sync the changeset checks since we alwasy have at max one worker thread
                return Compiler._changeSet.Select(e => e.Id).SequenceEqual(Changeset.Select(e => e.Id));
            }
        }

        // id might be: "scenario", "factory", or vpath of a node
        public Type PrevSuccessfulType(String id)
        {
            var types = Compiler._types.Where(t => t.Attr<SeqAttribute>().Seq != WorkerSeq).ToArray();

            if (id == "scenario")
            {
                var scenarios = types.Where(t => typeof(ICompiledScenario).IsAssignableFrom(t)).Reverse();
                return scenarios.FirstOrDefault(t => t.Attr<SeqAttribute>().Seq == LastSuccessfulWorkerSeq);
            }
            else if (id == "factory")
            {
                var scenario = PrevSuccessfulType("scenario");
                var seq = scenario == null ? -1 : scenario.Attr<SeqAttribute>().Seq;
                return scenario == null ? null : types.SingleOrDefault(t => t.Name.Contains("_factory_seq" + seq));
            }
            else
            {
                var nodes = types.Where(t => typeof(ICompiledNode).IsAssignableFrom(t)).Reverse();
                nodes = nodes.Where(t => t.AttrOrNull<VPathAttribute>() != null);
                return nodes.FirstOrDefault(t => t.Attr<VPathAttribute>().VPath == new VPath(id));
            }
        }

        public Dictionary<IElement, EventReason> NormalizedChangeSet
        {
            get
            {
                // todo. tests:
                // 1) new fla: add, content -> add
                // 2) new fla x2: add, content, add, content -> add
                // 3) new fla, then cancel the dialog: add, remove -> nothing

                var map = new Dictionary<IElement, EventReason>();
                foreach (var e in Changeset)
                {
                    if (e.Reason == EventReason.Remove)
                    {
                        if (map.ContainsKey(e.Subject))
                        {
                            // a remove event overrides previously registered self-events
                            var reason = map[e.Subject];
                            map.Remove(e.Subject);

                            // remove also self-destroys if there was an [add this] event previously
                            // i.e. add X -> ... -> remove X = annihilation
                            if (reason == EventReason.Add)
                            {
                                continue;
                            }
                        }

                        // a remove event overrides previously registered self- and child-related events
                        var b = e.Subject as IBranch;
                        if (b != null)
                        {
                            foreach (var kvp in map.ToArray())
                            {
                                if (kvp.Key.Parents().Contains(b))
                                {
                                    map.Remove(kvp.Key);
                                }
                            }
                        }
                    }

                    // if this session processes addition/removal of the parent, there's no need
                    // to also process any action with its children separately

                    if (map.Any(kvp => (
                           kvp.Value == EventReason.Add || kvp.Value == EventReason.Remove) &&
                           e.Subject.Parents().Contains(kvp.Key as IBranch)))
                    {
                        continue;
                    }

                    // if we haven't been filtered out, then add the record to the history
                    // btw, this record might be removed later

                    // remove X -> add X sequence won't annihilate, but the add will rather overwrite the remove
                    // todo: this means that when processing add the compiler should also check whether the node was removed
                    // from any of previously compiled nodes' children collection, and adjust those if necessary
                    map[e.Subject] = e.Reason;
                }

                return map;
            }
        }

        private Dictionary<IBranch, CumulativeOperation[]> _scheduledCumulations = null;
        public Dictionary<IBranch, CumulativeOperation[]> ScheduledCumulations
        {
            get
            {
                if (_scheduledCumulations == null)
                {
                    _scheduledCumulations = new Dictionary<IBranch, CumulativeOperation[]>();
                    Action<KeyValuePair<IElement, EventReason>, IBranch> schedule = (kvp, host) =>
                    {
                        if (!_scheduledCumulations.ContainsKey(host)) _scheduledCumulations.Add(host, new CumulativeOperation[0]);
                        var mutable = new List<CumulativeOperation>(_scheduledCumulations[host]);
                        mutable.Add(new CumulativeOperation(host, kvp.Key, kvp.Value));
                        _scheduledCumulations[host] = mutable.ToArray();
                    };

                    foreach (var kvp in NormalizedChangeSet)
                    {
                        var subject = kvp.Key;
                        var reason = kvp.Value;

                        if (subject is IBranch)
                        {
                            if (reason == EventReason.Add)
                            {
                                if (subject.IsFov())
                                {
                                    // recompile the node that hosts this svd/fla
                                    // 1) override CreateProperties to add a new property
                                    // 2) add a new field/property that'd host the svd/fla
                                    schedule(kvp, subject.Parent.Parent);
                                }
                                else
                                {
                                    // recompile the parent node
                                    // 1) override CreateChildren to add a new child
                                    // 2) also add a new field/property that'd host the new node
                                    // 3) then proceed to compiling the class corresponding to the node
                                    // no special registration in node factory is necessary: the latter step will auto-do that
                                    schedule(kvp, subject.Parent);
                                }
                            }
                            else if (reason == EventReason.Remove)
                            {
                                if (subject.IsFov())
                                {
                                    // recompile the node that hosts this svd/fla
                                    // 1) just override the corresponding property to crash as CompiledNode.Eval(vpath) does
                                    schedule(kvp, subject.Parent.Parent);
                                }
                                else
                                {
                                    // do not recompile anything
                                    // 1) just unregister the node and its children from NodeFactory
                                    var cc = (CompilationContext)this;
                                    cc.Factory.il()
                                        .ldarg(0)
                                        .ldstr(subject.VPath)
                                        .newobj(typeof(VPath), typeof(String))
                                        .callvirt(typeof(NodeFactory).GetMethod("UnregisterRecursively", BF.All));
                                }
                            }
                            else
                            {
                                throw new NotSupportedException(String.Format(
                                    "The event '{0}' for the node '{1}' is unsupported at this stage.",
                                    reason, subject));
                            }
                        }
                        else if (subject is IValue)
                        {
                            if (reason == EventReason.Content)
                            {
                                if (subject.Name == "elfCode" && subject.Parent.IsFormula())
                                {
                                    // recompile the node that hosts this formula
                                    // 1) just override the corresponding property and compile the new elfCode
                                    schedule(kvp, subject.Parent.Parent.Parent);
                                }
                                else
                                {
                                    throw new NotSupportedException(String.Format(
                                        "The event '{0}' for the node '{1}' is unsupported at this stage.",
                                        reason, subject));
                                }
                            }
                            else
                            {
                                throw new NotSupportedException(String.Format(
                                    "The event '{0}' for the node '{1}' is unsupported at this stage.",
                                    reason, subject));
                            }
                        }
                        else
                        {
                            throw new NotSupportedException(String.Format(
                                "The event '{0}' for the node '{1}' is unsupported at this stage.",
                                reason, subject));
                        }
                    }
                }

                return _scheduledCumulations;
            }
        }
    }
}