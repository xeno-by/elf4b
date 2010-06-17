using System;
using System.Diagnostics;
using System.Linq;
using Playground.Helpers;

namespace Playground
{
    public static class Codifier
    {
        private static String RepositoryTemplate
        {
            get
            {
                return
                    "using System;" + Environment.NewLine +
                    "using System.Collections.Generic;" + Environment.NewLine +
                    "using System.Globalization;" + Environment.NewLine +
                    "using System.Linq;" + Environment.NewLine +
                    "using System.Reflection;" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "public static class Repository" + Environment.NewLine +
                    "{{" + Environment.NewLine +
                    "    private static Dictionary<String, String> _dictionary = new Dictionary<String, String>();" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "    public static String GetValue(String name)" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        var props = typeof(Repository).GetProperties(BindingFlags.Public | BindingFlags.Static);" + Environment.NewLine +
                    "        var prop = props.SingleOrDefault(prop1 => prop1.Name == name);" + Environment.NewLine +
                    "        if (prop == null)" + Environment.NewLine +
                    "            throw new ArgumentException(String.Format(\"Repository doesn't have property named '{{0}}'\", name));" + Environment.NewLine +
                    "        if (!prop.CanRead)" + Environment.NewLine +
                    "            throw new ArgumentException(String.Format(\"Cannot read property '{{0}}'\", name));" + Environment.NewLine +
                    "        return (String)prop.GetValue(null, null);" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "    public static void SetValue(String name, String value)" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        var props = typeof(Repository).GetProperties(BindingFlags.Public | BindingFlags.Static);" + Environment.NewLine +
                    "        var prop = props.SingleOrDefault(prop1 => prop1.Name == name);" + Environment.NewLine +
                    "        if (prop == null)" + Environment.NewLine +
                    "            throw new ArgumentException(String.Format(\"Repository doesn't have property named '{{0}}'\", name));" + Environment.NewLine +
                    "        if (!prop.CanWrite)" + Environment.NewLine +
                    "            throw new ArgumentException(String.Format(\"Cannot write property '{{0}}'\", name));" + Environment.NewLine +
                    "        prop.SetValue(null, value, null);" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "    private static double Parse(String value)" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        var pp = value.Replace(\"%\", \"\").Replace(\" \", \"\").Replace(\",\", \".\");" + Environment.NewLine +
                    "        var val = double.Parse(pp, CultureInfo.InvariantCulture);" + Environment.NewLine +
                    "        return value.Contains(\"%\") ? val / 100 : val;" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "    private static String _emptyOrError = \"\";" + Environment.NewLine +
                    "" + Environment.NewLine + "{0}" +
                    "}}" + Environment.NewLine;
            }
        }

        private static String FormulaTemplate
        {
            get
            {
                return
                    "public static String {0}" + Environment.NewLine +
                    "{{" + Environment.NewLine +
                    "    get" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        try" + Environment.NewLine +
                    "        {{" + Environment.NewLine +
                    "            return {1};" + Environment.NewLine +
                    "        }}" + Environment.NewLine +
                    "        catch" + Environment.NewLine +
                    "        {{" + Environment.NewLine +
                    "            return _emptyOrError;" + Environment.NewLine +
                    "        }}" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "}}" + Environment.NewLine;
            }
        }

        private static String VariableTemplate
        {
            get
            {
                return
                    "public static String {0}" + Environment.NewLine +
                    "{{" + Environment.NewLine +
                    "    get" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        if(_dictionary.ContainsKey(\"{0}\"))" + Environment.NewLine +
                    "            return _dictionary[\"{0}\"];" + Environment.NewLine +
                    "        return _emptyOrError;" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "" + Environment.NewLine +
                    "    set" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        _dictionary[\"{0}\"] = value;" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "}}" + Environment.NewLine;
            }
        }

        private static String ЁVarTemplate
        {
            get
            {
                return
                    "public static String {0}" + Environment.NewLine +
                    "{{" + Environment.NewLine +
                    "    get" + Environment.NewLine +
                    "    {{" + Environment.NewLine +
                    "        if(_dictionary.ContainsKey(\"{0}\"))" + Environment.NewLine +
                    "            return _dictionary[\"{0}\"];" + Environment.NewLine +
                    "        return _emptyOrError;" + Environment.NewLine +
                    "    }}" + Environment.NewLine +
                    "}}" + Environment.NewLine;
            }
        }

        public static String CodifyFormulae(String[] formulae, String[] initializedVars, String[] ёvars)
        {
            var formulaeProps = formulae.ToDictionary(
                f => f.Substring(0, f.IndexOf("=")).Trim(), 
                f => f.Substring(f.IndexOf("=") + 1).Trim());
            formulaeProps.Remove("С50"); // С50=У50*С4/М1
            var formulaeCodegen = formulaeProps.ToDictionary(
                kvp => kvp.Key,
                kvp => String.Format(FormulaTemplate, kvp.Key, "(" + kvp.Value + ").ToString(CultureInfo.InvariantCulture)"));

            var variableProps = formulae.Select(f => Extractor.DetectVariablesInFormula(f)).Flatten().Concat(initializedVars).Distinct();
            variableProps = variableProps.Except(formulaeProps.Keys).Except("округл".AsArray()).Except("удельныйвесэлемента".AsArray());
            var variableCodegen = variableProps.ToDictionary(v => v, v => String.Format(VariableTemplate, v));

            var ёvarProps = ёvars.Except(variableProps).Except(formulaeProps.Keys);
            var ёvarCodegen = ёvarProps.ToDictionary(v => v, v => String.Format(ЁVarTemplate, v));

            Trace.WriteLine(String.Empty);
            Trace.WriteLine(String.Format("formulae = [{0}]", formulaeCodegen.Keys.OrderBy(v => v).StringJoin()));
            Trace.WriteLine(String.Format("get/set vars = [{0}]", variableCodegen.Keys.OrderBy(v => v).StringJoin()));
            Trace.WriteLine(String.Format("readonly ёvars = [{0}]", ёvarCodegen.Keys.OrderBy(v => v).StringJoin()));

            var codegen = formulaeCodegen.Concat(variableCodegen).Concat(ёvarCodegen).OrderBy(kvp => kvp.Key)
                .Select(kvp => kvp.Value).StringJoin(Environment.NewLine).Indent(2);
            codegen = codegen.Replace("Parse(округл)", "Math.Floor");
            codegen = codegen.Replace("*Parse(удельныйвесэлемента),(/100)", "");

            return String.Format(RepositoryTemplate, codegen);
        }
    }
}
