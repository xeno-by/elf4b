using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Elf.Core.Reflection
{
    public class ElfClass : ReflectionEntity
    {
        public String Name { get; private set; }
        public ElfClass Rtimpl { get; private set; }
        public List<ClrMethod> Ctors { get; private set; }
        public List<ElfMethod> Methods { get; private set; }

        public Type ClrType { get; private set; }
        public Type ScopeResolver { get; private set; }
        public Type InvocationResolver { get; private set; }

        public ElfClass(AstNode elfNode, String name, Type clrType)
            : base(elfNode)
        {
            Name = name;
            Ctors = new List<ClrMethod>();
            Methods = new List<ElfMethod>();

            ClrType = clrType;
            ScopeResolver = ClrType.GetScopeResolver();
            InvocationResolver = ClrType.GetInvocationResolver();
        }

        public ElfClass(AstNode elfNode, String name, ElfClass rtimpl)
            : this(elfNode, name, rtimpl.ClrType)
        {
            Rtimpl = rtimpl;
        }

        public override string ToString()
        {
            return String.Format("{0} (rtimpl: {1}, clr: {2}, sr: {3}, ir: {4})", 
                Name, Rtimpl == null ? null : Rtimpl.Name, ClrType, ScopeResolver, InvocationResolver);
        }

        public string Dump()
        {
            var sb = new StringBuilder().AppendLine(ToString());

            var allMethods = Ctors.Cast<ElfMethod>().Concat(Methods).OrderBy(m => m is ClrMethod ? 1 : 2);
            allMethods = allMethods.ThenBy(m => m.Name);
            var methods = allMethods.Select(method => method.Dump().Indent(1)).StringJoin(Environment.NewLine + Environment.NewLine);
            if (!methods.IsNullOrEmpty()) sb.Append(methods).AppendLine();

            return sb.AppendLine("end").ToString();
        }
    }
}
