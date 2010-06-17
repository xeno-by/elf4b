using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elf.Helpers;

namespace Elf.Core.Runtime.Contexts
{
    public class RuntimeContext : IVmBound
    {
        public VirtualMachine VM { get; private set; }
        public void Bind(VirtualMachine vm) { VM = vm; }

        public Stack<NativeCallContext> CallStack { get; set; }
        public ClrCallContext PendingClrCall { get; set; }

        public RuntimeContext()
        {
            CallStack = new Stack<NativeCallContext>();
            PendingClrCall = null;
        }

        public String Dump()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Pending CLR call = {0}", PendingClrCall == null ? null : PendingClrCall.Dump()).AppendLine();
            sb.AppendLine("Native stack trace =").Append(
                CallStack.Select(sf => sf.Dump().Indent(1)).StringJoin(String.Empty));
            return sb.ToString();
        }
    }
}