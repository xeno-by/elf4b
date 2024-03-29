using System;
using System.Diagnostics;

namespace Esath.Data.Properties
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    [DebuggerNonUserCode]
    internal class AssemblyBuiltAtAttribute : Attribute
    {
        public String Timestamp { get; set; }

        public AssemblyBuiltAtAttribute(){}
        public AssemblyBuiltAtAttribute(String timestamp) { Timestamp = timestamp; }
    }
}