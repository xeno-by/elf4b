using System;
using System.Collections.Generic;
using System.Reflection;
using DataVault.Core.Api;
using Elf.Core.ClrIntegration;
using System.Linq;
using Elf.Helpers;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Expressions;

namespace Esath.Pie.Helpers
{
    public static class ElfUtils
    {
        public static String ToElfIdentifier(this VPath vpath)
        {
            var s = vpath.ToString();
            return s.Substring(1).Replace(@"\", "$");
        }

        public static VPath FromElfIdentifier(this String var)
        {
            return var.Replace("$", @"\");
        }

        private static Dictionary<Assembly, Dictionary<String, int?>> _cacheRaw =
            new Dictionary<Assembly, Dictionary<String, int?>>();
        private static Dictionary<String, IEnumerable<int?>> _fxcache
        {
            get
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (asm.IsDefined(typeof(ElfDiscoverableAttribute), false))
                    {
                        if (!_cacheRaw.ContainsKey(asm))
                        {
                            var cache = new Dictionary<String, int?>();
                            asm.GetTypes().SelectMany(t => t
                                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                                .Where(m => m.RtimplOf() != null))
                                // todo. not correct for script host methods
                                .ForEach(m => cache.Add(m.RtimplOf(), m.IsVarargs() ? null : (int?)m.GetParameters().Length));
                            _cacheRaw.Add(asm, cache);
                        }
                    }
                }

                var d = _cacheRaw.Select(kvp => kvp.Value).SelectMany(kvp => kvp).GroupBy(
                    kvp => kvp.Key, kvp => kvp.Value).ToDictionary(e => e.Key, e => (IEnumerable<int?>)e);
                d.Add("?", ((int?)null).AsArray());
                d.Add("=", ((int?)2).AsArray());
                return d;
            }
        }

        public static bool CanFreezeArgc(this String fxname)
        {
            return _fxcache.ContainsKey(fxname) && 
                _fxcache[fxname].Where(i => i != null).Distinct().Count() == 1;
        }

        public static int? GuessArgc(this String fxname)
        {
            if (_fxcache.ContainsKey(fxname))
            {
                var distinct = _fxcache[fxname].Distinct();
                return distinct.Count() == 1 ? distinct.Single() : null;
            }
            else
            {
                return null;
            }
        }

        public static PieType? PieType(this AstNode node)
        {
            var expr = node as Expression;
            if (expr != null)
            {
                if (expr is InvocationExpression || expr is AssignmentExpression) return Helpers.PieType.Fx;
                if (expr is LiteralExpression) return Helpers.PieType.Const;
                if (expr is VariableExpression)
                {
                    var var = (VariableExpression)expr;
                    var isVar = var.Name == "?" || var.Name.Contains("_sourceValueDeclarations") ||
                        var.Name.Contains("_formulaDeclarations");
                    return isVar ? Helpers.PieType.Var : Helpers.PieType.Node;
                }
            }

            return null;
        }

        public static bool IsFov(this IElement e)
        {
            return e is IBranch && ((IBranch)e).IsFov();
        }

        public static bool IsFov(this IBranch b)
        {
            return b.IsFormula() || b.IsSvd();
        }

        public static bool IsFormula(this IElement e)
        {
            return e is IBranch && ((IBranch)e).IsFormula();
        }

        public static bool IsFormula(this IBranch b)
        {
            return b.Parents().Any(p => p.Name == "_formulaDeclarations");
        }

        public static bool IsSvd(this IElement e)
        {
            return e is IBranch && ((IBranch)e).IsSvd();
        }

        public static bool IsSvd(this IBranch b)
        {
            return b.Parents().Any(p => p.Name == "_sourceValueDeclarations");
        }

        public static bool IsFormulaHost(this IElement e)
        {
            return e is IBranch && ((IBranch)e).IsFormulaHost();
        }

        public static bool IsFormulaHost(this IBranch b)
        {
            return b.Name == "_formulaDeclarations";
        }

        public static bool IsSvdHost(this IElement e)
        {
            return e is IBranch && ((IBranch)e).IsSvdHost();
        }

        public static bool IsSvdHost(this IBranch b)
        {
            return b.Name == "_sourceValueDeclarations";
        }
    }
}