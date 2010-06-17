using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Cola.Parameters;
using Elf.Core;
using Elf.Core.Assembler;
using Elf.Core.Reflection;
using Elf.Exceptions.Abstract;
using Elf.Helpers;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Expressions;
using Elf.Syntax.Ast.Statements;
using Elf.Syntax.AstBuilders;
using QuickGraph;
using QuickGraph.Algorithms;

namespace Elf.Cola
{
    public class ColaBottle
    {
        public ColaNode Cap { get; private set; }

        public ColaBottle(ColaNode cap)
        {
            Cap = cap;
        }

        public ParametersUsages ParamUsages
        {
            get
            {
                var usages = Cap.Flatten(c => c.Children, c => {
                    if (c.Script.IsNullOrEmpty()) return null;

                    Script script;
                    ElfVmInstruction[] asm;

                    try
                    {
                        var canon = c.Script.ToCanonicalCola();
                        script = (Script)new ElfAstBuilder(canon).BuildAst();

                        var vm = new VirtualMachine();
                        vm.Load(canon);
                        var className = canon.Substring(0, canon.NthIndexOf(" ", 2)).Substring(4);
                        var classDef = vm.Classes.Single(c2 => c2.Name == className);
                        asm = ((NativeMethod)classDef.Methods.Single()).Body;
                    }
                    catch(ErroneousScriptException)
                    {
                        return null;
                    }

                    var interactive = script.Children.ElementAt(0).Children.ElementAt(0);
                    var flat = interactive.Flatten(node => node.Children).ToArray();

                    Func<String, bool> isExternal = s => 
                        !flat.OfType<VarStatement>().Any(decl => decl.Name == s);
                    Func<String, String> unqualify = s => 
                        s.StartsWith(c.TPath + ".") ? s.Substring(s.LastIndexOf(".") + 1) : s;
                    Func<String, int> usageOf = s => asm.Select(evi =>
                    {
                        var pushRef = evi as PushRef;
                        if (pushRef != null && (pushRef.Ref == s || pushRef.Ref == unqualify(s)))
                        {
                            return +1;
                        }

                        var popRef = evi as PopRef;
                        if (popRef != null && (popRef.Ref == s || popRef.Ref == unqualify(s)))
                        {
                            return -1;
                        }

                        return 0;
                    }).FirstOrDefault(i => i != 0);


                    Func<String, String> qualify = s => s.Contains(".") ? s : c.TPath + "." + s;
                    var variables = flat.OfType<VariableExpression>().Select(v => qualify(v.Name)).Distinct();
                    return variables.Where(isExternal).ToDictionary(qualify, usageOf);
                }).Where(kvp => kvp.Value != null);

                var exported = new ParametersUsages();

                var declaredParams = Cap.Flatten(n => n.Children).Select(n => n.ParamValues.Keys
                    .Select(p => new Parameter(p.Id, n.TPath + "." + p.Name))).Flatten().Distinct();
                declaredParams.ForEach(p => exported.Add(p, new ParameterUsage(p)));

                usages.ForEach(kvp => kvp.Value.ForEach(kvp2 => {
                    if (!exported.ContainsKey(kvp2.Key))
                    {
                        var newp = new Parameter(kvp2.Key);
                        exported.Add(newp, new ParameterUsage(newp));
                    }

                    if (kvp2.Value == -1) exported[kvp2.Key].Out.Add(kvp.Key);
                    if (kvp2.Value == +1) exported[kvp2.Key].In.Add(kvp.Key);
                }));

                return exported;
            }
        }

        public AdjacencyGraph<ColaNode, Edge<ColaNode>> DependencyGraph
        {
            get
            {
                var g = new AdjacencyGraph<ColaNode, Edge<ColaNode>>();

                var flat = Cap.Flatten(n => n.Children);
                flat.ForEach(n => g.AddVertex(n));

                var usages = ParamUsages;
                flat.ForEach(n1 => flat.ForEach(n2 => {
                   if (n1 != n2 && usages.Any(p => p.Value.In.Contains(n2) && p.Value.Out.Contains(n1)))
                       g.AddEdge(new Edge<ColaNode>(n1, n2));
                }));

                return g;
            }
        }

        public IEnumerable<ColaNode> ExecutionPlan
        {
            get
            {
                return DependencyGraph.TopologicalSort();
            }
        }

        private ColaNode Select(String tpath)
        {
            var indices = tpath.Split('.');
            if (indices.FirstOrDefault() != Cap.Name)
            {
                return null;
            }
            else
            {
                return indices.Skip(1).Aggregate(Cap, (curr, s) =>
                    curr == null ? null : curr.Children.SingleOrDefault(c => c.Name == s));
            }
        }

        public ParametersValues ParamValues
        {
            get
            {
                var values = new ParametersValues();
                var flat = Cap.Flatten(n => n.Children);
                flat.ForEach(n => n.ParamValues.ForEach(v => values.Add(
                    new Parameter(v.Key.Id, n.TPath + "." + v.Key.Name), v.Value)));
                return values;
            }

            set
            {
                Cap.Flatten(n => n.Children).ForEach(n => n.ParamValues.Clear());

                foreach(var kvp in value)
                {
                    var delim = kvp.Key.Name.LastIndexOf(".");
                    var tpath = kvp.Key.Name.Substring(0, Math.Max(0, delim));
                    var unqualified = kvp.Key.Name.Substring(delim + 1);

                    var p_unqualified = new Parameter(kvp.Key.Id, unqualified);
                    Select(tpath).ParamValues.Add(p_unqualified, kvp.Value);
                }
            }
        }

        public override string ToString()
        {
            return "[" + Cap.Flatten(n => n.Children).StringJoin() + "]";
        }
    }
}