using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using Playground.Helpers;

namespace Playground
{
    public static class Extractor
    {
        private static Regex _validFormulaeRegex;
        public static Regex ValidFormulaeRegex
        {
            get
            {
                if (_validFormulaeRegex == null)
                {
                    _validFormulaeRegex = new Regex(
                        @"^[%_0-9a-zA-Zа-яА-ЯёЁ]*=[_.,\(\)\s\+\-\*/%0-9a-zA-Zа-яА-ЯёЁ]+$",
                        RegexOptions.Compiled);
                }

                return _validFormulaeRegex;
            }
        }

        private static bool[] _validFormulaeSymbols;
        public static bool[] ValidFormulaeSymbols
        {
            get
            {
                if (_validFormulaeSymbols == null)
                {
                    var sb = new StringBuilder("=+-*/%().,");
                    sb.Append(" \f\n\r\t\v");
                    for (var c = '0'; c <= '9'; c++) sb.Append(c);
                    for (var c = 'a'; c <= 'z'; c++) sb.Append(c);
                    for (var c = 'A'; c <= 'Z'; c++) sb.Append(c);
                    for (var c = 'а'; c <= 'я'; c++) sb.Append(c);
                    for (var c = 'А'; c <= 'Я'; c++) sb.Append(c);
                    sb.Append("_ёЁ");

                    _validFormulaeSymbols = new bool[Char.MaxValue];
                    var validChars = sb.ToString().ToCharArray();
                    for(var c = Char.MinValue; c < Char.MaxValue; ++c)
                        _validFormulaeSymbols[c] = validChars.Contains(c);
                }

                return _validFormulaeSymbols;
            }
        }

        public static Regex ValidVariableRegex
        {
            get
            {
                return new Regex(@"^[_a-zA-Zа-яА-ЯёЁ][_%0-9a-zA-Zа-яА-ЯёЁ]*$");
            }
        }

        public static Regex NumberPercentageRegex
        {
            get
            {
                return new Regex(@"^\d*%$");
            }
        }

        public static Char[] ValidVariableSymbols
        {
            get
            {
                var sb = new StringBuilder("%");
                for (var c = '0'; c <= '9'; c++) sb.Append(c);
                for (var c = 'a'; c <= 'z'; c++) sb.Append(c);
                for (var c = 'A'; c <= 'Z'; c++) sb.Append(c);
                for (var c = 'а'; c <= 'я'; c++) sb.Append(c);
                for (var c = 'А'; c <= 'Я'; c++) sb.Append(c);
                sb.Append("_ёЁ");
                return sb.ToString().ToCharArray();
            }
        }

        public static Regex ValidSpecialVariableRegex
        {
            get
            {
                return new Regex(@"^ё[_%0-9a-zA-Zа-яА-ЯёЁ]+$");
            }
        }

        private static bool[] _validSpecialVariableSymbols;
        public static bool[] ValidSpecialVariableSymbols
        {
            get
            {
                if (_validSpecialVariableSymbols == null)
                {
                    var sb = new StringBuilder("%");
                    for (var c = '0'; c <= '9'; c++) sb.Append(c);
                    for (var c = 'a'; c <= 'z'; c++) sb.Append(c);
                    for (var c = 'A'; c <= 'Z'; c++) sb.Append(c);
                    for (var c = 'а'; c <= 'я'; c++) sb.Append(c);
                    for (var c = 'А'; c <= 'Я'; c++) sb.Append(c);
                    sb.Append("_ёЁ");

                    _validSpecialVariableSymbols = new bool[Char.MaxValue];
                    var validChars = sb.ToString().ToCharArray();
                    for (var c = Char.MinValue; c < Char.MaxValue; ++c)
                        _validSpecialVariableSymbols[c] = validChars.Contains(c);
                }

                return _validSpecialVariableSymbols;
            }
        }

        public static IEnumerable<String> DetectFormulae(String text)
        {
            var nodup = FilterDuplicateFormulae(DetectFormulaeImpl(text).ToArray()).ToArray();
            return nodup.Select(f => PostprocessFormula(f));
        }

        public static IEnumerable<String> DetectFormulaeImpl(String text)
        {
            var sb = new StringBuilder();

//            var sw = Stopwatch.StartNew();
//            var i = 0;

            foreach(var c in text)
            {
                if (ValidFormulaeSymbols[c])
                {
                    sb.Append(c);
                }
                else
                {
                    // todo. will fail if two formulae are split only with whitespaces
                    var s = sb.ToString();
                    if (sb.Length >= 0 && ValidFormulaeRegex.IsMatch(s))
                    {
                        Func<char, bool> isLatin = c2 => ('a' <= c2 && c2 <= 'z') || ('A' <= c2 && c2 <= 'Z');
                        if (s.Any(isLatin))
                        {
                            var i = 0;
                            throw new NotSupportedException(String.Format(
                                "A wannabe formula '{0}' has non-russian symbols at position(s) [{1}]",
                                s, s.ToCharArray()
                                    .Select(c2 => new { c = c2, i = i++ })
                                    .Where(sm => isLatin(sm.c)).Select(sm => sm.i).StringJoin()));
                        }

                        yield return s;
                    }

                    sb = new StringBuilder();
                }

//                ++i;
//                if (i % (text.Length / 10) == 0)
//                {
//                    Trace.WriteLine(String.Format("{0}0% completed, {1} time spent",
//                        i / (text.Length / 10), sw.Elapsed));
//                    sw = Stopwatch.StartNew();
//                }
            }

            var s2 = sb.ToString();
            if (sb.Length >= 0 && ValidFormulaeRegex.IsMatch(s2))
            {
                Func<char, bool> isLatin = c2 => ('a' <= c2 && c2 <= 'z') || ('A' <= c2 && c2 <= 'Z');
                if (s2.Any(isLatin))
                {
                    var i = 0;
                    throw new NotSupportedException(String.Format(
                        "A wannabe formula '{0}' has non-russian symbols at position(s) [{1}]",
                        s2, s2.ToCharArray()
                            .Select(c2 => new { c = c2, i = i++ })
                            .Where(sm => isLatin(sm.c)).Select(sm => sm.i).StringJoin()));
                }

                yield return s2;
            }
        }

        private static IEnumerable<String> FilterDuplicateFormulae(String[] formulae)
        {
            var titles = formulae.Select(f => f.Substring(0, f.IndexOf("="))).ToArray();
            for(var i = 0; i < formulae.Length; ++i)
            {
                var original = titles.Take(i).IndexOf(titles[i]);
                if (original == -1)
                {
                    yield return formulae[i];
                }
                else
                {
                    Trace.WriteLine(String.Format("Ignoring '{0}': it has the same title as '{1}'", 
                        formulae[i], formulae[original]));
                }
            }

            Trace.WriteLine(String.Empty);
        }

        private static String PostprocessFormula(String formula)
        {
            var orig = formula;
            formula = Regex.Replace(formula, @"\s", "");

            var pp = new StringBuilder();
            var sb = new StringBuilder();
            foreach (var c in formula)
            {
                if (ValidVariableSymbols.Contains(c))
                {
                    sb.Append(c);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        if (ValidVariableRegex.IsMatch(sb.ToString()))
                        {
                            if (pp.Length == 0)
                            {
                                pp.Append(sb.Replace('%', '_'));
                            }
                            else
                            {
                                pp.Append("Parse(" + sb.Replace('%', '_') + ")");
                            }
                        }
                        else if (NumberPercentageRegex.IsMatch(sb.ToString()))
                        {
                            sb.Remove(sb.Length - 1, 1);
                            pp.Append("(" + sb + "/100)");
                        }
                        else
                        {
                            pp.Append(sb);
                        }

                        sb = new StringBuilder();
                    }

                    pp.Append(c);
                }
            }

            if (sb.Length > 0)
            {
                if (ValidVariableRegex.IsMatch(sb.ToString()))
                {
                    if (pp.Length == 0)
                    {
                        pp.Append(sb.Replace('%', '_'));
                    }
                    else
                    {
                        pp.Append("Parse(" + sb.Replace('%', '_') + ")");
                    }
                }
                else if (NumberPercentageRegex.IsMatch(sb.ToString()))
                {
                    sb.Remove(sb.Length - 1, 1);
                    pp.Append("(" + sb + "/100)");
                }
                else
                {
                    pp.Append(sb);
                }
            }

            formula = pp.ToString();
            Trace.WriteLine(String.Format("{0} -> {1}", orig, formula));
            return formula;
        }

        public static IEnumerable<String> DetectVariablesInDocument(String text)
        {
            var sb = new StringBuilder();

            foreach (var c in text)
            {
                if (ValidSpecialVariableSymbols[c])
                {
                    sb.Append(c);
                }
                else
                {
                    var s = sb.ToString();
                    if (sb.Length > 0 && sb[0] == 'ё' && ValidSpecialVariableRegex.IsMatch(s))
                    {
                        Func<char, bool> isLatin = c2 => ('a' <= c2 && c2 <= 'z') || ('A' <= c2 && c2 <= 'Z');
                        if (s.Any(isLatin))
                        {
                            var i = 0;
                            throw new NotSupportedException(String.Format(
                                "A wannabe ё-variable '{0}' has non-russian symbols at position(s) [{1}]",
                                s, s.ToCharArray()
                                    .Select(c2 => new { c = c2, i = i++ })
                                    .Where(sm => isLatin(sm.c)).Select(sm => sm.i).StringJoin()));
                        }

                        yield return s.Substring(1);
                    }

                    sb = new StringBuilder();
                }
            }

            var s2 = sb.ToString();
            if (sb.Length > 0 && sb[0] == 'ё' && ValidSpecialVariableRegex.IsMatch(s2))
            {
                Func<char, bool> isLatin = c2 => ('a' <= c2 && c2 <= 'z') || ('A' <= c2 && c2 <= 'Z');
                if (s2.Any(isLatin))
                {
                    var i = 0;
                    throw new NotSupportedException(String.Format(
                        "A wannabe ё-variable '{0}' has non-russian symbols at position(s) [{1}]",
                        s2, s2.ToCharArray()
                            .Select(c2 => new { c = c2, i = i++ })
                            .Where(sm => isLatin(sm.c)).Select(sm => sm.i).StringJoin()));
                }

                yield return s2.Substring(1);
            }
        }

        public static IEnumerable<String> DetectVariablesInFormula(String formula)
        {
            var fla = Regex.Match(formula, @"^(?<title>.*?)=(?<expr>.*)$");
            if (fla.Success)
            {
                yield return fla.Result("${title}");

                for (var match = Regex.Match(fla.Result("${expr}"), @"Parse\((?<name>.*?)\)");
                    match.Success; match = match.NextMatch())
                {
                    yield return match.Result("${name}");
                }
            }
        }

        public static IEnumerable<String> DetectVariablesInCS(String csProg)
        {
            foreach(var line in csProg.SelectLines())
            {
                if (line.Contains("="))
                {
                    var match = Regex.Match(line, @"^\s*Repository\.(?<name>.*?) = " + "\".*?\";$");
                    if (match.Success)
                    {
                        yield return match.Result("${name}");
                    }
                    else
                    {
                        throw new NotSupportedException(String.Format("Unsupported line format: '{0}'", line));
                    }
                }
            }
        }
    }
}
