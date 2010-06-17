using System;
using System.Collections.Generic;
using Elf.Cola.Analysis;
using Elf.Cola.Exceptions;
using Elf.Cola.Facta;
using Elf.Cola.Parameters;
using Elf.Core;
using Elf.Exceptions;
using Elf.Exceptions.Abstract;
using Elf.Helpers;
using System.Linq;

namespace Elf.Cola
{
    public class CocaContext : ColaBottle
    {
        private VirtualMachine VM { get; set; }

        public CocaContext(ColaNode cap) 
            : base(cap) 
        {
            VM = new VirtualMachine();
        }

        public Findings InspectComposition()
        {
            try
            {
                return new Findings(InspectCompositionImpl());
            }
            catch(Exception e)
            {
                if (e is ElfException) throw;
                if (e is CocacolaException) throw;
                throw new UnexpectedCocacolaException(String.Format(
                    "Unexpected error inspecting composition. Reason: '{0}'.", e.Message), e);
            }
        }

        public Findings InspectCompositionAndParameters()
        {
            try
            {
                return new Findings(InspectCompositionImpl().Concat(InspectParametersImpl()));
            }
            catch(Exception e)
            {
                if (e is ElfException) throw;
                if (e is CocacolaException) throw;
                throw new UnexpectedCocacolaException(String.Format(
                    "Unexpected error inspecting parameters. Reason: '{0}'.", e.Message), e);
            }
        }

        private IEnumerable<Factum> InspectCompositionImpl()
        {
            foreach(var node in Cap.Flatten(n => n.Children).Where(n => !n.Script.IsNullOrEmpty()))
            {
                var e = TryLoadElfLight(node.Script);
                if (e != null) yield return new ScriptIsErroneousFactum(node, e);
            }

            var cycle = DependencyGraph.GetAnyOfExistingLoops();
            if (!cycle.IsNullOrEmpty())
            {
                yield return new DependencyGraphHasLoopFactum(this, cycle);
            }
        }

        private ErroneousScriptException TryLoadElfLight(String elfLight)
        {
            try
            {
                // todo. regularly purge no more useful classes
                VM.Load(elfLight.ToCanonicalCola());
            }
            catch (ErroneousScriptException e)
            {
                return e;
            }

            return null;
        }

        private IEnumerable<Factum> InspectParametersImpl()
        {
            var usages = ParamUsages;
            var values = ParamValues;
            var plan = ExecutionPlan;

            // todo. param value can also be non-deterministically present (a warning factum)
            // this might happen if the value is set only in a certain branch of execution

            var required = usages.Where(kvp => kvp.Value.Out.IsNullOrEmpty()).Select(kvp => kvp.Key);
            foreach(var p_required in required)
            {
                if (!values.ContainsKey(p_required) || values[p_required] == null)
                    yield return new ParameterValueIsMissingFactum(p_required, usages[p_required].In);
            }

            // todo. to be honest, here we should also check for assignments to parameters provided
            // if we don't do this, calculations might spawn a vicious loop of re-updates

            Func<Parameter, int> firstUsageOf = p =>
                plan.Select(n => usages[p].UsedIn(n)).FirstOrDefault(i => i != 0);
            Func<Parameter, bool> hasValue = p => values.ContainsKey(p) && values[p] != null;

            var valueNotUsed = usages.Keys.Where(p => hasValue(p) && firstUsageOf(p) == -1);
            foreach (var p_valnotused in valueNotUsed)
            {
                yield return new ParameterValueIsNeverUsedFactum(p_valnotused, this);
            }

            var mutatedSeveralTimes = usages.Where(kvp => kvp.Value.Out.Count > 1).Select(kvp => kvp.Key);
            foreach(var p_mutated in mutatedSeveralTimes)
            {
                yield return new ParameterIsMutatedSeveralTimesFactum(p_mutated, usages[p_mutated].Out);
            }
        }

        // used because one can't pass a local variable as a ref to a lambda setter
        private ParametersValues _tempValues;

        public ChangeSet Eval()
        {
            try
            {
                var findings = InspectCompositionAndParameters();
                if (findings.AreFatal)
                {
                    throw new FatalFindingsException(findings);
                }
                else
                {
                    _tempValues = ParamValues;
                    return ExecutionPlan.Aggregate(
                        new ChangeSet(() => ParamValues, v => { ParamValues = v; }).StartRecording().Capture(),
                        (curr, n) => curr.Merge(Eval(n)));
                }
            }
            catch(Exception e)
            {
                if (e is CocacolaException) throw;
                throw new UnexpectedCocacolaException(String.Format(
                    "Unexpected error evaluating complex calculation. Reason: '{0}'.", e.Message), e);
            }
        }

        private ChangeSet Eval(ColaNode node)
        {
            var canonical = node.Script.ToCanonicalCola();
            var className = canonical.Substring(0, canonical.NthIndexOf(" ", 2)).Substring(4);

            // todo. regularly purge no more useful classes
            VM.Load(canonical);

            VM.Context["cc_values"] = _tempValues;
            VM.Context["cc_node"] = node;

            var changeset = new ChangeSet(() => _tempValues, v => { _tempValues = v; }).StartRecording();

            try
            {
                VM.CreateEntryPoint(className, "Main").RunTillEnd();
            }
            catch(ErroneousScriptException e)
            {
                // todo. a good idea would be to continue evaluation of everything that doesn't depend on this node
                var findings = InspectCompositionAndParameters();
                throw new FatalFindingsException(new Findings(
                    findings.Concat(new ScriptIsErroneousFactum(node, e).AsArray())));
            }

            return changeset.Capture();
        }
    }
}