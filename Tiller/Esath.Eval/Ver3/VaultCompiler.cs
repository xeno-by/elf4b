using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DataVault.Core.Api;
using DataVault.Core.Helpers;
using DataVault.Core.Api.Events;
using Elf.Core;
using Elf.Core.Assembler;
using Elf.Core.Assembler.Literals;
using Elf.Core.Reflection;
using Elf.Core.Runtime.Impl;
using Elf.Core.TypeSystem;
using Esath.Data;
using Esath.Data.Core;
using Esath.Eval.Ver3.Async;
using Esath.Eval.Ver3.Core;
using Esath.Eval.Ver3.Helpers;
using Esath.Eval.Ver3.Snippets;
using System.Linq;
using Esath.Pie.Helpers;
using Version=Esath.Eval.Ver3.Core.Version;
using Elf.Syntax.Light;
using Label=System.Reflection.Emit.Label;

namespace Esath.Eval.Ver3
{
	using DataVault.Core.Helpers.Assertions;
	using DataVault.Core.Helpers.Reflection;
	using DataVault.Core.Helpers.Reflection.Shortcuts;

	// todo. review the code for more expected exceptions, see also todo around line 594
    // namely: bad reference inside elfCode (to an unexisting node, that was e.g. deleted by the user)
    // the scenario described above should crash in run-time, but not in compile-time
    // todo. would be nice to have our own hierarchy of exceptions instead of notimplemented and notsupported

    // todo. profiling:
    // 1) performance: are async calls really fast to be unnoticed by the user? are barrier locks/unlocks really fast?
    // 2) memory: what if we get rid of all attributes? how much memory will it save?

    // todo. unit-tests
    // while, they're not implemented, here are some scenarios;
    // 1) Just run and wait for the compiler to finish (maybe, click here and there between the "Preview" and "Conditions" tabs)
    // 2) 1, wait, then create new formula: open dialog, enter name, press next, then cancel
    // 3) 2, but the last step is to create a valid formula
    // 4/5) 2/3, but in rapid succession so that the baseline compiler of 1 doesn't finish executing

    // for running those do the following:
    // 1) uncomment the following lines in StartUp.cs: "MainImmediatelyActivatePreviewWithPoorPerformance();" and "return;"
    // 2) create the "mtlog" directory in your application's folder (there you'll find thread logs generated, "null" is the UI thread)

    // todo. more tests:
    // 1) Remove the node, and then add a node with the same name (node, but not fla/svd)
    // 2) Same for flae/svd, but several times to check correct overriding and new'ing: 
    //    add > remove > add > edit > remove > add with other type

    public partial class VaultCompiler : IVaultCompiler
    {
        public IVault Vault { get; private set; }

        // no need to sync access to the builder since at max only one worker will use the builder
        internal AssemblyBuilder _asm;
        internal ModuleBuilder _mod;
        internal List<Type> _types = new List<Type>();

        public VaultCompiler(IVault vault)
        {
            Vault = vault.AssertNotNull();
            Vault.Changed(SignificantChangeFilter, SignificantChangeProcessor);
            InitializeCompiler();
        }

        private void InitializeCompiler()
        {
            lock (_barrier)
            {
                if (_asm == null)
                {
                    try
                    {
#if TRACE
                        _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(Vault.GetAssemblyName()), AssemblyBuilderAccess.RunAndSave);
                        _mod = _asm.DefineDynamicModule(Vault.GetAssemblyName() + ".dll", true);

                        // do not use DomainUnload here because it never gets fired for default domain
                        AppDomain.CurrentDomain.ProcessExit += (o, e) =>
                        {
                            try
                            {
                                if (_asm != null)
                                {
                                    _asm.Save(_asm.GetName().Name + ".dll");
                                }
                            }
                            catch(Exception ex)
                            {
                                MTLog.Say("Asm dumper has faced an unexpected exception: " + ex);
                            }
                        };
#else
                        _asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(Vault.GetClassName()), AssemblyBuilderAccess.Run);
                        _mod = _asm.DefineDynamicModule(Vault.GetClassName() + ".dll", false);
#endif
                        _asm.SetCustomAttribute(new CustomAttributeBuilder(
                            typeof(GuidAttribute).GetConstructors().Single(),
                            Vault.Id.ToString().MkArray()));
                    }
                    catch (Exception ex)
                    {
                        MTLog.Say("Compiler initialization failed: " + ex);
                        _isPermanentlyBroken = true;
                    }
                }
            }

            GetCompiledAsync();
        }

        private Type CompileScenarioRecursively(CompilationContext ctx)
        {
#if FORCE_SINGLE_THREADED
            Application.DoEvents();
#endif
            // vault modifications guard
            if (!ctx.StillInTouch()) throw new CompilerOutOfTouchException();

            // type
            var t_parent = ctx.PrevSuccessfulType("scenario") ?? typeof(CompiledScenario);
            (t_parent == typeof(CompiledScenario) ^ ctx.CumulativeCompilation).AssertTrue();
            var t = _mod.DefineType(ctx.RequestName(Vault.GetClassName()), TA.Public, t_parent);
#if TRACE
            try
            {
#endif
                // constructors
                var ctorDesignMode = t.DefineConstructor(MA.PublicCtor, CallingConventions.Standard,
                    new[] { typeof(IVault) });
                ctorDesignMode.DefineParameter(1, ParmA.None, "scenario");
                ctorDesignMode.il().ld_args(2).basector(typeof(CompiledScenario), typeof(IVault)).ret();
                var ctorRuntimeMode = t.DefineConstructor(MA.PublicCtor, CallingConventions.Standard,
                    new[] { typeof(IVault), typeof(IVault) });
                ctorRuntimeMode.DefineParameter(1, ParmA.None, "scenario");
                ctorRuntimeMode.DefineParameter(2, ParmA.None, "repository");
                ctorRuntimeMode.il().ld_args(3).basector(typeof(CompiledScenario), typeof(IVault), typeof(IVault)).ret();

                // the [Version(...)] attribute for reflection-based analysis
                t.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(VersionAttribute).GetConstructor(new[] { typeof(String), typeof(ulong) }),
                    new object[] { Vault.Id.ToString(), Vault.Revision }));

                // the [Seq(...)] attribute for synchronization and namespacing purposes
                t.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(SeqAttribute).GetConstructor(new[] { typeof(int) }),
                    new object[] { ctx.WorkerSeq }));

                // trivial overrides
                t.DefineOverrideReadonly(typeof(CompiledScenario).GetProperty("Version"))
                    .il()
                    .ldstr(Vault.Id.ToString())
                    .newobj(typeof(Guid), typeof(String))
                    .ldc_i8(Vault.Revision)
                    .newobj(typeof(Version), typeof(Guid), typeof(ulong))
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Name"))
                    .il()
                    .ldstr(Vault.GetClassName() + "_seq" + ctx.WorkerSeq)
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("VPath"))
                    .il()
                    .call(typeof(VPath).GetProperty("Empty").GetGetMethod())
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Revision"))
                    .il()
                    .ldc_i8(Vault.Revision)
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Seq"))
                    .il()
                    .ldc_i4(ctx.WorkerSeq)
                    .ret();

                // factory initialization
                var parentFactory = ctx.PrevSuccessfulType("factory") ?? typeof(NodeFactory);
                ctx.FactoryType = _mod.DefineType(ctx.RequestName(t.Name + "_factory"), TA.Public, parentFactory);
                ctx.FactoryType.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(SeqAttribute).GetConstructor(new[] { typeof(int) }),
                    new object[] { ctx.WorkerSeq }));

                // setting up the factory method
                ctx.Factory = ctx.FactoryType.DefineConstructor(MA.PublicCtor, CallingConventions.Standard, new Type[0]);
                ctx.Factory.il().ldarg(0).basector(parentFactory);

                // complex overrides (the main work is here, below the stack)
                if (ctx.CompilingBaseline)
                {
                    // baseline compilation involves creating classes and properties 
                    // for every single node and svd/fla in the scenario
                    ImplementCreateChildren(ctx, t, Vault);
                    ImplementCreateProperties(ctx, t, Vault);
                }
                else
                {
                    var cum = ctx.ScheduledCumulations;
                    MTLog.Say(String.Format("Scheduled cumulations: [{0}]{1}",
                        cum.Count,
                        cum.Count == 0 ? String.Empty : Environment.NewLine +
                            cum.Select(kvp =>
                                String.Format("[{0}] {1}{2}",
                                    kvp.Value.Length,
                                    kvp.Key,
                                    cum.Count == 0 ? String.Empty : Environment.NewLine +
                                        kvp.Value.Select(co =>
                                            String.Format(">>{0}: {1}", co.Reason, co.Subject)
                                        ).StringJoin(Environment.NewLine))
                            ).StringJoin(Environment.NewLine)));

                    // cumulative compilation only should regenerate pieces of code 
                    // that are affected by events mentioned in ctx.NormalizedChangeSet
                    cum.Keys.ForEach(host => CompileNode(ctx, host));
                }

                // factory finalization
                ctx.Factory.il().ret();
                var t_factory_created = ctx.FactoryType.CreateType();
                _types.Add(t_factory_created);
                var createNodeFactory = t.DefineOverride(typeof(CompiledScenario).GetMethod("CreateNodeFactory", BF.All));
                createNodeFactory.il().newobj(t_factory_created).ret();

                // finalize the type
                var t_created = t.CreateType();
                _types.Add(t_created);
                return t_created;
#if TRACE
            }
            finally
            {
                // attempts to finalize the type so that we can dump an assembly to disk even after exceptions
                if (!t.IsCreated())
                {
                    Action<Action> neverFail = code => {try{code();}catch{}};
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledScenario).GetProperty("Version")).il().ldc_i4(0).ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Name")).il().ldnull().ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("VPath")).il().ldnull().ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Revision")).il().ldc_i8(0).ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Seq")).il().ldc_i4(0).ret());
                    neverFail(() => t.DefineOverride(typeof(CompiledNode).GetMethod("CreateChildren", BF.All)).il().ret());
                    neverFail(() => t.DefineOverride(typeof(CompiledNode).GetMethod("CreateProperties", BF.All)).il().ret());
                    neverFail(() => ctx.CCs[t].il().ret());
                    neverFail(() => ctx.CPs[t].il().ret());
                    neverFail(() => t.DefineOverride(typeof(CompiledScenario).GetMethod("CreateNodeFactory", BF.All)).il().ldnull().ret());
                    neverFail(() => t.CreateType());
                }

                if (!ctx.FactoryType.IsCreated())
                {
                    Action<Action> neverFail = code => {try{code();}catch{}};
                    neverFail(() => ctx.Factory.il().ret());
                    neverFail(() => ctx.FactoryType.CreateType());
                }
            }
#endif
        }

        private Type CompileNode(CompilationContext ctx, IBranch b)
        {
#if FORCE_SINGLE_THREADED
            Application.DoEvents();
#endif
            // vault modifications guard
            if (!ctx.StillInTouch()) throw new CompilerOutOfTouchException();

            // type
            var t_parent = ctx.PrevSuccessfulType(b.VPath) ?? typeof(CompiledNode);
            (t_parent == typeof(CompiledNode) ^ (ctx.CumulativeCompilation && ctx.ScheduledCumulations.ContainsKey(b))).AssertTrue();
            var t = _mod.DefineType(ctx.RequestName(b.GetClassName()), TA.Public, t_parent);

#if TRACE
            try
            {
#endif
                // constructor
                var ctor = t.DefineConstructor(MA.PublicCtor, CallingConventions.Standard, typeof(CompiledNode).MkArray());
                ctor.DefineParameter(1, ParmA.None, "parent");
                ctor.il().ld_args(2).basector(typeof(CompiledNode), typeof(CompiledNode)).ret();

                // the [VPath(...)] attribute for reflection-based analysis
                t.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(VPathAttribute).GetConstructor(typeof(String).MkArray()),
                    b.VPath.ToString().MkArray()));

                // the [Revision(...)] attribute for reflection-based analysis
                t.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(RevisionAttribute).GetConstructor(new[] { typeof(ulong) }),
                    new object[] { Vault.Revision }));

                // the [Seq(...)] attribute for synchronization and namespacing purposes
                t.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(SeqAttribute).GetConstructor(new[] { typeof(int) }),
                    new object[] { ctx.WorkerSeq }));

                // trivial overrides
                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Name"))
                    .il()
                    .ldstr(b.GetClassName() + "_seq" + ctx.WorkerSeq)
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("VPath"))
                    .il()
                    .ldstr(b.VPath)
                    .newobj(typeof(VPath), typeof(String))
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Revision"))
                    .il()
                    .ldc_i8(Vault.Revision)
                    .ret();

                t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Seq"))
                    .il()
                    .ldc_i4(ctx.WorkerSeq)
                    .ret();

                ImplementCreateChildren(ctx, t, b);
                ImplementCreateProperties(ctx, t, b);

                // finalize the type
                var t_created = t.CreateType();
                _types.Add(t_created);

                // create a factory method for this class
                var factoryName = b.GetClassName().Substring(b.GetClassName().LastIndexOf(".") + 1);
                var factoryMethod = ctx.FactoryType.DefineMethod("Create_" + ctx.RequestName(factoryName), MA.PublicStatic, typeof(CompiledNode), typeof(CompiledNode).MkArray());
                factoryMethod.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(VPathAttribute).GetConstructor(typeof(String).MkArray()),
                    b.VPath.ToString().MkArray()));
                factoryMethod.il().ldarg(0).newobj(t_created, typeof(CompiledNode).MkArray()).ret();

                // register self within the factory
                ctx.Factory.il()
                    .ldarg(0)
                    .ldstr(b.VPath.ToString())
                    .newobj(typeof(VPath), typeof(String))
                    .ldnull()
                    .ldftn(factoryMethod)
                    .newobj(typeof(Func<CompiledNode, CompiledNode>), typeof(Object), typeof(IntPtr))
                    .callvirt(typeof(NodeFactory).GetMethod("Register", BF.All));

                return t_created;
#if TRACE
            }
            finally
            {
                // attempts to finalize the type so that we can dump an assembly to disk after certain exceptions
                if (!t.IsCreated())
                {
                    Action<Action> neverFail = code => {try{code();}catch{}};
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Name")).il().ldnull().ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("VPath")).il().ldnull().ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Revision")).il().ldc_i8(0).ret());
                    neverFail(() => t.DefineOverrideReadonly(typeof(CompiledNode).GetProperty("Seq")).il().ldc_i4(0).ret());
                    neverFail(() => t.DefineOverride(typeof(CompiledNode).GetMethod("CreateChildren", BF.All)).il().ret());
                    neverFail(() => t.DefineOverride(typeof(CompiledNode).GetMethod("CreateProperties", BF.All)).il().ret());
                    neverFail(() => ctx.CCs[t].il().ret());
                    neverFail(() => ctx.CPs[t].il().ret());
                    neverFail(() => t.CreateType());
                }
            }
#endif
        }

        private void ImplementCreateChildren(CompilationContext ctx, TypeBuilder t, IBranch root)
        {
            var relevantCumulations = ctx.ScheduledCumulations.ContainsKey(root) ? 
                ctx.ScheduledCumulations[root].Where(co => !(co.Subject.IsFov() || co.Subject.Parent.IsFov())) : null;
            var special = ctx.CumulativeCompilation && relevantCumulations != null;
            if (special && relevantCumulations.IsEmpty()) return;

            var base_cc = t.BaseType.GetMethod("CreateChildren", BF.All);
            var cc = t.DefineOverride(base_cc);
            if (!base_cc.IsAbstract) cc.il().ldarg(0).call(base_cc);
            ctx.CCs[t] = cc;

            var svd = root.GetBranches().Where(b => b.Name == "_sourceValueDeclarations");
            var formulae = root.GetBranches().Where(b => b.Name == "_formulaDeclarations");
            var conditions = root.GetBranches().Where(b => b.Name == "_conditions");
            var nodes = root.GetBranches().Except(svd).Except(formulae).Except(conditions).ToArray();
            if (special) nodes = nodes.Where(b => relevantCumulations.Any(co => co.Subject == b)).ToArray();

            foreach (var b in nodes)
            {
                var b_type = CompileNode(ctx, b);

                var f_child = t.DefineField("_" + b.GetPropertyName().ToLower(), b_type, FA.Private);
                var p_child = t.DefineProperty(b.GetPropertyName(), PropA.None, b_type, new Type[0]);
                p_child.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(VPathAttribute).GetConstructor(typeof(String).MkArray()),
                    b.VPath.ToString().MkArray()));

                var get = t.DefineMethod("get_" + b.GetPropertyName(), MA.PublicProp, b_type, new Type[0]);
                p_child.SetGetMethod(get);

                Label nodeWasCreatedByTheFactory;
                get.il()
                   .ldarg(0)
                   .ldfld(f_child)

                   // here we check whether the factory returned a valid (non-null) node
                   // if the latter is false, then the node has been deleted during cumulative recompilation
                   // and we need to crash with the same message as CompiledNode.Child(vpath)
                   .def_label(out nodeWasCreatedByTheFactory)
                   .brtrue(nodeWasCreatedByTheFactory)
                   .ldstr(String.Format("There's no compiled node at VPath '{0}'.", b.VPath))
                   .@throw(typeof(NotImplementedException), typeof(String))

                   .label(nodeWasCreatedByTheFactory)
                   .ldarg(0)
                   .ldfld(f_child)
                   .ret();

                cc.il()
                  .ldarg(0)
                  .ldarg(0)
                  .callvirt(typeof(CompiledNode).GetProperty("Root", BF.All).GetGetMethod(true))
                  .ldstr(b.VPath.ToString())
                  .newobj(typeof(VPath), typeof(String))
                  .ldarg(0)
                  .callvirt(typeof(CompiledScenario).GetMethod("CreateNode", BF.All))
                  .stfld(f_child)
                  .ldarg(0)
                  .callvirt(typeof(CompiledNode).GetProperty("Children").GetGetMethod())
                  .ldarg(0)
                  .ldfld(f_child)
                  .callvirt(typeof(CompiledNodeCollection).GetMethod("Add"));
            }

            cc.il().ret();
        }

        private void ImplementCreateProperties(CompilationContext ctx, TypeBuilder t, IBranch root)
        {
            var relevantCumulations = ctx.ScheduledCumulations.ContainsKey(root) ?
                ctx.ScheduledCumulations[root].Where(co => co.Subject.IsFov() || co.Subject.Parent.IsFov()) : null;
            var special = ctx.CumulativeCompilation && relevantCumulations != null;
            if (special && relevantCumulations.IsEmpty()) return;

            var base_cp = t.BaseType.GetMethod("CreateProperties", BF.All);
            var cp = t.DefineOverride(base_cp);
            if (!base_cp.IsAbstract) cp.il().ldarg(0).call(base_cp);
            ctx.CPs[t] = cp;

            var svd = root.GetBranches().Where(b => b.Name == "_sourceValueDeclarations");
            var formulae = root.GetBranches().Where(b => b.Name == "_formulaDeclarations");
            var conditions = root.GetBranches().Where(b => b.Name == "_conditions");
            var nodes = root.GetBranches().Except(svd).Except(formulae).Except(conditions);
            var svdAndFormulae = svd.Concat(formulae).SelectMany(b => b.GetBranches()).ToArray();
            if (special) svdAndFormulae = svdAndFormulae.Where(b => 
                relevantCumulations.Any(co => co.Subject == b || co.Subject.Parent == b)).ToArray();

            var propGetCache = new Dictionary<VPath, MethodInfo>();
            var fieldCache = new Dictionary<VPath, FieldBuilder>();
            var nameCache = new HashSet<String>();
            Action<IBranch, bool> ensureProperty = (b, external) =>
            {
                if (propGetCache.ContainsKey(b.VPath))
                {
                    return;
                }
                else
                {
                    var typeToken = b.GetValue("type").ContentString;
                    var propType = typeToken.GetTypeFromToken();

                    var baseProp = t.BaseType.GetProperties(BF.All).SingleOrDefault(
                        p => p.HasAttr<VPathAttribute>() && p.Attr<VPathAttribute>().VPath == b.VPath);
                    var basePropOk = baseProp != null && baseProp.PropertyType == propType;

                    String name;
                    if (basePropOk)
                    {
                        name = baseProp.Name;
                    }
                    else
                    {
                        var desiredName = b.GetPropertyName();
                        name = desiredName;

                        var i = 0;
                        while (nameCache.Contains(name))
                        {
                            name = desiredName + "~" + ++i;
                        }

                        nameCache.Add(b.GetPropertyName());
                    }

                    if (external)
                    {
                        if (basePropOk)
                        {
                            propGetCache.Add(b.VPath, baseProp.GetGetMethod(true));
                        }
                        else
                        {
                            var p_prop = t.DefineProperty(name, PropA.None, propType, new Type[0]);
                            p_prop.SetCustomAttribute(new CustomAttributeBuilder(
                                typeof(VPathAttribute).GetConstructor(typeof(String).MkArray()),
                                b.VPath.ToString().MkArray()));

                            var get = t.DefineMethod("get_" + name, MA.ProtectedProp, propType, new Type[0]);
                            get.il()
                               .ldarg(0)
                               .callvirt(typeof(CompiledNode).GetProperty("Root").GetGetMethod())
                               .ldstr(b.VPath.ToString())
                               .newobj(typeof(VPath), typeof(String))
                               .callvirt(typeof(ICompiledNode).GetMethod("Eval"))
                               .ret();

                            p_prop.SetGetMethod(get);
                            propGetCache.Add(b.VPath, get);
                        }
                    }
                    else
                    {
                        var p_prop = t.DefineProperty(name, PropA.None, propType, new Type[0]);
                        p_prop.SetCustomAttribute(new CustomAttributeBuilder(
                            typeof(VPathAttribute).GetConstructor(typeof(String).MkArray()),
                            b.VPath.ToString().MkArray()));

                        MethodBuilder get;
                        if (basePropOk)
                        {
                            var baseGet = baseProp.GetGetMethod(true);
                            get = t.DefineOverride(baseGet);
                            p_prop.SetGetMethod(get);
                        }
                        else
                        {
                            get = t.DefineMethod("get_" + name, MA.PublicProp, propType, new Type[0]);
                            p_prop.SetGetMethod(get);
                        }

                        propGetCache.Add(b.VPath, get);
                        fieldCache.Add(b.VPath, t.DefineField("_" + name.ToLower(), propType, FA.Private));
                    }
                }
            };

            Action<IBranch> updateCp = b =>
            {
                var get = propGetCache[b.VPath];
                cp.il()
                  .ldarg(0)
                  .callvirt(typeof(CompiledNode).GetProperty("Properties").GetGetMethod())
                  .ldarg(0)
                  .ldstr(b.GetPropertyName())
                  .ldstr(b.VPath)
                  .newobj(typeof(VPath), typeof(String))
                  .ldarg(0)
                  .ldftn(get)
                  .newobj(typeof(Func<IEsathObject>), typeof(Object), typeof(IntPtr))
                  .newobj(typeof(CompiledProperty), typeof(CompiledNode), typeof(String), typeof(VPath), typeof(Func<IEsathObject>))
                  .callvirt(typeof(CompiledPropertyCollection).GetMethod("Add"));
            };

            // handle deleted flae and svds i.e. create CompiledNode.Eval-like crash
            if (special)
            {
                foreach (IBranch deleted in relevantCumulations.Where(co => co.Reason == EventReason.Remove).Select(co => co.Subject))
                {
                    ensureProperty(deleted, false);
                    var get = (MethodBuilder)propGetCache[deleted.VPath];
                    get.il()
                       .ldstr(String.Format("There's no compiled property at VPath '{0}'.", deleted.VPath))
                       .@throw(typeof(NotImplementedException), typeof(String));
                    updateCp(deleted);
                }
            }

            // define all properties in advance so that we can reference them when needed
            svdAndFormulae.ForEach(b => ensureProperty(b, false));

            // implement defined properties
            foreach (var b in svdAndFormulae)
            {
                var typeToken = b.GetValue("type").ContentString;
                var propType = typeToken.GetTypeFromToken();

                var f_prop = fieldCache[b.VPath];
                var get = (MethodBuilder)propGetCache[b.VPath];

                // if a formula has just been created (i.e. has null elfCode), then we just generate notimplemented stuff
                // todo. this is a particular case of graceful dealing with invalid elf code
                // a solid approach would also handle such stuff as: non-existing references, resolving to invalid methods
                // and i think something else (needs thorough checking)
                // note. when implementing that stuff, be sure to burn the failboat elf code + the failure reason right into the assembly code
                // so that one can analyze the formula by him/herself and find the reason of the failure

                if (b.IsFormula())
                {
                    var host = b.GetValue("elfCode");
                    var code = host == null ? null : host.ContentString;
                    var elfCode = code == null ? null : code.ToCanonicalElf();

                    if (elfCode == null)
                    {
                        get.il().@throw(typeof(NotImplementedException));
                        updateCp(b);
                        continue;
                    }
                }

                Label cacheOk;
                LocalBuilder loc_cache, loc_vpath, loc_result;
                get.il()
                   .ldarg(0)
                   .callvirt(typeof(CompiledNode).GetProperty("Root").GetGetMethod())
                   .ldfld(typeof(CompiledScenario).GetField("CachedPropertiesRegistry", BF.All))
                   .def_local(typeof(HashSet<VPath>), out loc_cache)
                   .stloc(loc_cache)
                   .ldstr(b.VPath.ToString())
                   .newobj(typeof(VPath), typeof(String))
                   .def_local(typeof(VPath), out loc_vpath)
                   .stloc(loc_vpath)
                   .ldloc(loc_cache)
                   .ldloc(loc_vpath)
                   .callvirt(typeof(HashSet<VPath>).GetMethod("Contains"))
                   .def_label(out cacheOk)
                   .brtrue(cacheOk)

                   // here svd and flae codegen will store the evaluation result
                   .def_local(propType, out loc_result);

                if (b.IsSvd())
                {
                    Label isRuntime, stlocRuntime, isDesignTime, stlocDesignTime, coalesce;
                    LocalBuilder runtime, designTime;
                    get.il()

                        // runtime -> value is stored in the repository
                       .ldarg(0)
                       .callvirt(typeof(CompiledNode).GetProperty("Repository", BF.All).GetGetMethod(true))
                       .def_label(out stlocRuntime)
                       .def_label(out isRuntime)
                       .def_local(typeof(String), out runtime)
                       .brtrue_s(isRuntime)
                       .ldnull()
                       .br_s(stlocRuntime)
                       .label(isRuntime)
                       .ldarg(0)
                       .callvirt(typeof(CompiledNode).GetProperty("Repository", BF.All).GetGetMethod(true))
                       .ldarg(0)
                       .callvirt(typeof(CompiledNode).GetProperty("Scenario", BF.All).GetGetMethod(true))
                       .ldstr(b.VPath.ToString())
                       .newobj(typeof(VPath), typeof(String))
                       .callvirt(typeof(CachedVault).GetMethod("GetBranch"))
                       .ldstr("repositoryValue")
                       .newobj(typeof(VPath), typeof(String))
                       .callvirt(typeof(IBranch).GetMethod("GetValue"))
                       .callvirt(typeof(IValue).GetProperty("ContentString").GetGetMethod())
                       .newobj(typeof(VPath), typeof(String))
                       .callvirt(typeof(CachedVault).GetMethod("GetValue"))
                       .callvirt(typeof(IValue).GetProperty("ContentString").GetGetMethod())
                       .label(stlocRuntime)
                       .stloc(runtime)

                        // design time -> value is stored directly in the scenario next to the node
                       .ldarg(0)
                       .callvirt(typeof(CompiledNode).GetProperty("Repository", BF.All).GetGetMethod(true))
                       .def_label(out stlocDesignTime)
                       .def_label(out isDesignTime)
                       .def_local(typeof(String), out designTime)
                       .brfalse_s(isDesignTime)
                       .ldnull()
                       .br_s(stlocDesignTime)
                       .label(isDesignTime)
                       .ldarg(0)
                       .callvirt(typeof(CompiledNode).GetProperty("Scenario", BF.All).GetGetMethod(true))
                       .ldstr(b.VPath.ToString())
                       .newobj(typeof(VPath), typeof(String))
                       .callvirt(typeof(CachedVault).GetMethod("GetBranch"))
                       .ldstr("valueForTesting")
                       .newobj(typeof(VPath), typeof(String))
                       .callvirt(typeof(IBranch).GetMethod("GetValue"))
                       .callvirt(typeof(IValue).GetProperty("ContentString").GetGetMethod())
                       .label(stlocDesignTime)
                       .stloc(designTime)

                       // prepare for convert
                       .ldtoken(propType)
                       .callvirt(typeof(Type).GetMethod("GetTypeFromHandle"))
                       .callvirt(typeof(Elf.Helpers.ReflectionHelper).GetMethod("ElfDeserializer"))

                        // coalesce designtime and runtime values
                       .ldloc(designTime)
                       .dup()
                       .def_label(out coalesce)
                       .brtrue_s(coalesce)
                       .pop()
                       .ldloc(runtime)
                       .label(coalesce)

                       // convert the string to esath object
                       .callvirt(typeof(Func<String, IElfObject>).GetMethod("Invoke"))
                       .stloc(loc_result);
                }
                else if (b.IsFormula())
                {
                    var host = b.GetValue("elfCode");
                    var code = host == null ? null : host.ContentString;
                    var elfCode = code == null ? null : code.ToCanonicalElf();
                    elfCode.AssertNotNull(); // this is guaranteed by the check above (at the start of this method)

                    Label retTarget;
                    get.il().def_label(out retTarget);

                    var vm = new VirtualMachine();
                    vm.Load(elfCode);
                    var evis = ((NativeMethod)vm.Classes.Last().Methods.Single()).Body;

                    Func<String, VPath> idToVpath = elfid =>
                    { try { return elfid.FromElfIdentifier(); } catch { return null; } };

                    Func<String, IEsathObject> elfStrToEsath = elfStr =>
                    { try { return elfStr.FromStorageString(); } catch { return null; } };

                    var il = get.il();
                    var evalStack = new Stack<Type>();

                    Type auxResultType = null;
                    for (var j = 0; j < evis.Length; j++)
                    {
                        var evi = evis[j];
                        if (evi is Decl)
                        {
                            throw new NotSupportedException(
                                String.Format("The 'decl' instructions is not supported."));
                        }
                        else if (evi is Dup)
                        {
                            if (j < evis.Length - 1 && evis[j + 1] is PopRef)
                            {
                                // compile [dup, popref, pop] combo as simply [popref]
                                // since so far constructs like a = b = c are not used
                            }
                            else
                            {
                                var dupe = evalStack.Peek();
                                evalStack.Push(dupe);
                                il.dup();
                            }
                        }
                        else if (evi is Enter)
                        {
                            // do nothing
                        }
                        else if (evi is Invoke)
                        {
                            var invoke = (Invoke)evi;
                            var args = new Type[invoke.Argc];
                            for (var i = invoke.Argc - 1; i >= 0; --i)
                                args[i] = evalStack.Pop();

                            var resolved = DefaultInvocationResolver.Resolve(
                                vm,
                                invoke.Name,
                                vm.Classes.Last(),
                                args.Select(arg => vm.Classes.Single(c => c.Name == Elf.Helpers.ReflectionHelper.RtimplOf(arg))).ToArray());
                            if (resolved == null)
                            {
                                throw new NotSupportedException(String.Format(
                                    "The '{0}' instruction is not supported. " +
                                    "Cannot resolve invocation '{1}({2}).",
                                    invoke, invoke.Name, args.Select(arg => Elf.Helpers.ReflectionHelper.RtimplOf(arg)).StringJoin()));
                            }
                            else
                            {
                                if (!(resolved is ClrMethod))
                                {
                                    throw new NotSupportedException(String.Format(
                                        "The '{0}' instruction is not supported. " +
                                        "Invocation '{1}({2}) has been resolved to a native method '{3}'.",
                                        invoke, invoke.Name, args.Select(arg => Elf.Helpers.ReflectionHelper.RtimplOf(arg)).StringJoin(), resolved));
                                }
                                else
                                {
                                    var clrMethod = (MethodInfo)((ClrMethod)resolved).Rtimpl;
                                    if (clrMethod.ReturnType == typeof(void))
                                    {
                                        throw new NotSupportedException(String.Format(
                                            "The '{0}' instruction is not supported. " +
                                            "Invocation '{1}({2}) has been resolved to a void-returning method '{3}'.",
                                            invoke, invoke.Name, args.Select(arg => Elf.Helpers.ReflectionHelper.RtimplOf(arg)).StringJoin(), clrMethod));
                                    }
                                    else
                                    {
                                        il.callvirt(clrMethod);
                                        evalStack.Push(clrMethod.ReturnType);
                                    }
                                }
                            }
                        }
                        else if (evi is Jf)
                        {
                            throw new NotSupportedException(
                                String.Format("The 'jf' instruction is not supported."));
                        }
                        else if (evi is Jt)
                        {
                            throw new NotSupportedException(
                                String.Format("The 'jt' instruction is not supported."));
                        }
                        else if (evi is Elf.Core.Assembler.Label)
                        {
                            throw new NotSupportedException(
                                String.Format("The 'label' instruction is not supported."));
                        }
                        else if (evi is Leave)
                        {
                            // do nothing
                        }
                        else if (evi is Pop)
                        {
                            if (j > 0 && evis[j - 1] is PopRef)
                            {
                                // compile [dup, popref, pop] combo as simply [popref]
                                // since so far constructs like a = b = c are not used
                            }
                            else
                            {
                                il.pop();
                                evalStack.Pop();
                            }
                        }
                        else if (evi is PopAll)
                        {
                            evalStack.ForEach(t1 => { evalStack.Pop(); il.pop(); });
                        }
                        else if (evi is PopRef)
                        {
                            var popref = (PopRef)evi;

                            var vpath = idToVpath(popref.Ref);
                            if (vpath == null)
                            {
                                throw new NotSupportedException(String.Format(
                                    "The '{0}' instruction is not supported.", popref));
                            }
                            else
                            {
                                if (vpath != b.VPath)
                                {
                                    throw new NotSupportedException(String.Format(
                                        "The 'popref <vpath>' instruction is supported only " +
                                        "when vpath represents the current node. Current instruction " +
                                        "'{0}' is not supported", popref));
                                }
                                else
                                {
                                    auxResultType = evalStack.Pop();
                                    // todo. ensure that nothing significant follows this instruction
                                }
                            }
                        }
                        else if (evi is PushRef)
                        {
                            var pushref = (PushRef)evi;

                            var vpath = idToVpath(pushref.Ref);
                            if (vpath == null)
                            {
                                throw new NotSupportedException(String.Format(
                                    "The '{0}' instruction is not supported.", pushref));
                            }
                            else
                            {
                                if (!propGetCache.ContainsKey(vpath))
                                {
                                    var branch = Vault.GetBranch(vpath);
                                    if (branch == null)
                                    {
                                        throw new NotSupportedException(String.Format(
                                            "The 'pushref <vpath>' instruction is supported only " +
                                            "when vpath represents the svd or flae node. Current vpath '{0}' " +
                                            "doesn't reference any node in the scenario being compiled.", vpath));
                                    }

                                    if (!branch.IsFov())
                                    {
                                       throw new NotSupportedException(String.Format(
                                            "The 'pushref <vpath>' instruction is supported only " +
                                            "when vpath represents an svd or a flae node. Current branch " +
                                            "'{0}' at vpath '{1}' is not supported.", branch.Name, branch.VPath));
                                    }

                                    ensureProperty(branch, true);
                                }

                                var prop = propGetCache[vpath];
                                il.ldarg(0).callvirt(prop);
                                evalStack.Push(prop.ReturnType);
                            }
                        }
                        else if (evi is PushVal)
                        {
                            var pushval = (PushVal)evi;

                            if (!(pushval.Val is ElfStringLiteral) ||
                                (elfStrToEsath(((ElfStringLiteral)pushval.Val).Val)) == null)
                            {
                                throw new NotSupportedException(String.Format(
                                    "The 'pushval <val>' instruction is supported only " +
                                    "when val represents correctly encoded esath object. Current instruction " +
                                    "'{0}' is not supported", pushval));
                            }
                            else
                            {
                                var storageString = ((ElfStringLiteral)pushval.Val).Val;
                                var match = Regex.Match(storageString, @"^\[\[(?<token>.*?)\]\](?<content>.*)$");
                                match.Success.AssertTrue();

                                if (match.Success)
                                {
                                    var token = match.Result("${token}");
                                    var content = match.Result("${content}");

                                    il.ldtoken(token.GetTypeFromToken())
                                      .callvirt(typeof(Type).GetMethod("GetTypeFromHandle"))
                                      .callvirt(typeof(Elf.Helpers.ReflectionHelper).GetMethod("ElfDeserializer"))
                                      .ldstr(content)
                                      .callvirt(typeof(Func<String, IElfObject>).GetMethod("Invoke"));

                                    evalStack.Push(token.GetTypeFromToken());
                                }
                            }
                        }
                        else if (evi is Ret)
                        {
                            evalStack.IsEmpty().AssertTrue();
                            il.br_s(retTarget);
                        }
                        else
                        {
                            throw new NotSupportedException(String.Format(
                                "The '{0}' instruction is not supported.", evi));
                        }
                    }

                    if (auxResultType == propType)
                    {
                        il.label(retTarget)
                          .stloc(loc_result);
                    }
                    else
                    {
                        LocalBuilder auxResult;
                        var valPropGet = auxResultType.GetProperty("Val").GetGetMethod();

                        il.label(retTarget)
                          .def_local(auxResultType, out auxResult)
                          .stloc(auxResult)
                          .ldloc(auxResult)
                          .callvirt(valPropGet);

                        if (typeof(ElfNumber).IsAssignableFrom(auxResultType) &&
                            propType == typeof(EsathPercent))
                        {
                            // ldc_r8 is a must, since ldc_i4 performs integer multiplication
                            // and fails stuff like 1.2 * 100 (the result will be 100)

                            il.ldc_r8(100)
                              .mul();
                        }

                        il.convert(valPropGet.ReturnType, propType)
                          .stloc(loc_result);
                    }
                }
                else
                {
                    throw new NotSupportedException(
                        String.Format("Properties of type '{0}' are not supported", b));
                }

                get.il()
                   // the value is stored in a local after svd or flae codegen
                   .ldarg(0)
                   .ldloc(loc_result)
                   .stfld(f_prop)
                   .ldloc(loc_cache)
                   .ldloc(loc_vpath)
                   .callvirt(typeof(HashSet<VPath>).GetMethod("Add"))
                   .pop()

                   // if the value is cached, just execute this
                   .label(cacheOk)
                   .ldarg(0)
                   .ldfld(f_prop)
                   .ret();

                // finally register the property just compiled in the CreateProperties
                updateCp(b);
            }

            cp.il().ret();
        }
    }
}
