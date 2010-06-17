using System.Reflection;
using System.Linq;
using Elf.Syntax.Ast;
using Elf.Helpers;

namespace Elf.Core.Reflection
{
    public class ClrMethod : ElfMethod
    {
        public MethodBase Rtimpl { get; private set; }

        public ClrMethod(string name, MethodBase rtimpl)
            : this(null, null, name, rtimpl)
        {
        }

        public ClrMethod(AstNode elfNode, ElfClass declaringType, string name, MethodBase rtimpl)
            : base(elfNode, declaringType, name, rtimpl.GetParameters().Select(pi => pi.Name).ToArray(), rtimpl.IsVarargs()) 
        {
            Rtimpl = rtimpl;
        }

        protected override string DumpContent()
        {
            return Rtimpl.ToString();
        }
    }
}