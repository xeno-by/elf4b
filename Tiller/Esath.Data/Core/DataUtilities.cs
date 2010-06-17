using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Elf.Core.ClrIntegration;
using Elf.Helpers;
using Elf.Syntax.Ast.Expressions;
using Elf.Syntax.Grammar;

namespace Esath.Data.Core
{
    public static class DataUtilities
    {
        public static IEnumerable<Type> AllDataTypes
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetTypes().Where(t => !t.IsInterface && typeof(IEsathObject).IsAssignableFrom(t));
            }
        }

        public static String GetTypeToken(this Type t)
        {
            if (t == null) return null;
            if (!t.IsDefined(typeof(ElfSerializableAttribute), false)) return null;
            return t.GetCustomAttributes(typeof(ElfSerializableAttribute), false).
                Cast<ElfSerializableAttribute>().Single().TypeToken;
        }

        public static Type GetTypeFromToken(this String s)
        {
            return AllDataTypes.Where(t => t.GetTypeToken() == s).SingleOrDefault();
        }

        public static LocAttribute GetLocalization(this Type t)
        {
            if (t == null) return null;
            if (!t.IsDefined(typeof(LocAttribute), false)) return null;
            return t.GetCustomAttributes(typeof(LocAttribute), false).Cast<LocAttribute>().Single();
        }

        public static IEsathObject GetDefaultValue(this Type t)
        {
            if (t == typeof(EsathUndefined))
            {
                return new EsathUndefined();
            }
            else
            {
                var tval = t.GetProperty("Val").PropertyType;

                object val;
                if (tval.IsInterface || tval.IsClass)
                {
                    val = null;
                }
                else if (tval.MetadataToken == typeof(Nullable<>).MetadataToken &&
                    tval.Module == typeof(Nullable<>).Module)
                {
                    val = null;
                }
                else
                {
                    val = Activator.CreateInstance(tval);
                }

                var ctor = t.GetConstructors().Single();
                return (IEsathObject)ctor.Invoke(val.AsArray());
            }
        }

        public static IEsathObject FromUIString(this String s, Type t, CultureInfo locale)
        {
            return (IEsathObject)t.FromLocalString(s, locale);
        }

        public static IEsathObject FromStorageString(this String s)
        {
            var match = Regex.Match(s, @"^\[\[(?<token>.*?)\]\](?<content>.*)$");
            if (match.Success)
            {
                var token = match.Result("${token}");
                var parse = token.GetTypeFromToken().ElfDeserializer();
                return (IEsathObject)parse(match.Result("${content}"));
            }
            else
            {
                throw new NotSupportedException(String.Format(
                    "Cannot deserialize IEsathObject from the string '{0}'", s));
            }
        }

        public static String ToUIString(this IEsathObject eo, CultureInfo locale)
        {
            return eo.ToLocalString(locale);
        }

        public static String ToStorageString(this IEsathObject eo)
        {
            return String.Format("[[{0}]]{1}", eo.GetType().GetTypeToken(), eo.ToInvariantString());
        }

        public static IEsathObject AsEsathObject(this LiteralExpression le)
        {
            var data = le.Data.Unquote();
            if (le.Token != null && le.Token.Type == ElfParser.DecimalLiteral)
            {
                data = "[[number]]" + data;
            }
            else if (!Regex.IsMatch(data, @"^\[\[(?<token>.*?)\]\](?<content>.*)$"))
            {
                data = "[[text]]" + data;
            }

            return data.FromStorageString();
        }
    }
}