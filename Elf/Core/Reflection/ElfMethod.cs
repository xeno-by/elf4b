using System;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Elf.Core.Reflection
{
    public abstract class ElfMethod : ReflectionEntity
    {
        public ElfClass DeclaringType { get; private set; }
        public String Name { get; private set; }
        public String[] Args { get; private set; }
        public int Argc { get { return Args.Length; } }
        public bool IsVarargs { get; private set; }

        protected ElfMethod(AstNode elfNode, ElfClass declaringType, string name, String[] args, bool varargs) 
            : base(elfNode)
        {
            DeclaringType = declaringType;
            Name = name;
            Args = args;
            IsVarargs = varargs;
        }

        public override string ToString()
        {
            return String.Format("{0}({1}{2}) {3}, declared by {4}",
                Name, Args.StringJoin(), IsVarargs ? "..." : null,
                this is NativeMethod ? "native" : "rtimpl", DeclaringType == null ? "N/A" : DeclaringType.Name);
        }

        public virtual string Dump()
        {
            return String.Format("method {0}({1}{2}){3}{4}{3}end",
                Name, Args.StringJoin(), IsVarargs ? "..." : null, Environment.NewLine, DumpContent().Indent(1));
        }

        protected abstract string DumpContent();
    }
}