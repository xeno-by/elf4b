using System;
using System.Collections.Generic;
using System.Linq;
using DataVault.Core.Api;
using Elf.Helpers;
using Version=Esath.Eval.Ver3.Core.Version;

namespace Esath.Eval.Ver3.Helpers
{
    public static class ScenarioFormatUtils
    {
        private static String ExtractName(this IBranch b)
        {
            if (b is IVault)
            {
                throw new NotSupportedException();
            }
            else
            {
                var nameVal = b.GetValue("name");
                var name = nameVal == null ? b.Name : nameVal.ContentString;
                return name.Replace(".", "");
            }
        }

        public static String GetAssemblyName(this IVault vault)
        {
            var strippedUri = vault.Uri.Substring(vault.Uri.LastIndexOf(@"\") + 1);
            strippedUri = strippedUri.Substring(0, strippedUri.LastIndexOf("."));
            return strippedUri.Substring(strippedUri.LastIndexOf(".") + 1);
        }

        public static String GetClassName(this IBranch b)
        {
            if (b is IVault)
            {
                var vault = (IVault)b;
                return String.Format("{0}_rev{1}", vault.GetAssemblyName(), vault.Revision);
            }
            else
            {
                var @namespace = "Root".AsArray().Concat(b.Parents().Reverse().Skip(1).Select(b1 => b1.ExtractName())).StringJoin(".");
                var className = b.ExtractName() + "_rev" + b.Vault.Revision;

                if (@namespace.IsNullOrEmpty())
                {
                    return className;
                }
                else
                {
                    return @namespace + "." + className;
                }
            }
        }

        public static IEnumerable<IBranch> Parents(this IBranch b)
        {
            for (var current = b.Parent; current != null; current = current.Parent)
                yield return current;
        }

        public static String GetPropertyName(this IBranch b)
        {
            if (b is IVault)
            {
                throw new NotSupportedException();
            }
            else
            {
                return b.ExtractName();
            }
        }
    }
}