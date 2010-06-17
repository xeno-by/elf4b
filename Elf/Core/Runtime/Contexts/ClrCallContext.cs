using System;
using Elf.Core.TypeSystem;
using Elf.Core.Reflection;
using Elf.Helpers;

namespace Elf.Core.Runtime.Contexts
{
    public class ClrCallContext
    {
        public ClrMethod Source { get; set; }
        public IElfObject This { get; set; }
        public IElfObject[] Args { get; set; }

        public ClrCallContext(ClrMethod source, IElfObject @this, params IElfObject[] args) 
        {
            Source = source;
            This = @this;
            Args = args;
        }

        public String Dump()
        {
            return String.Format("{0}({1}) using {2} with this = {3}",
                Source.Name, Args.StringJoin(), Source.Rtimpl, This);
        }
    }
}