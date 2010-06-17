using System;
using System.Linq;
using System.Reflection;
using Esath.Data.Properties;

namespace Esath.Data.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LocAttribute : Attribute
    {
        private String TypeNameKey { get; set; }
        public String TypeName
        {
            get
            {
                return (String)typeof(Resources).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                    .Single(p => p.Name == TypeNameKey).GetValue(null, null);
            }
        }

        private String FormatErrorMessageKey { get; set; }
        public String FormatErrorMessage
        {
            get
            {
                return (String)typeof(Resources).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                    .Single(p => p.Name == FormatErrorMessageKey).GetValue(null, null);
            }
        }

        public LocAttribute(String typeNameKey, String formatErrorMessageKey)
        {
            TypeNameKey = typeNameKey;
            FormatErrorMessageKey = formatErrorMessageKey;
        }
    }
}