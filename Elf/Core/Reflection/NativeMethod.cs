using System;
using Elf.Core.Assembler;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Defs;
using System.Linq;
using Elf.Helpers;

namespace Elf.Core.Reflection
{
    public class NativeMethod : ElfMethod
    {
        public FuncDef FuncDef { get; private set; }
        public ElfVmInstruction[] Body { get; set; }

        public NativeMethod(AstNode elfNode, ElfClass declaringType)
            : base(elfNode, declaringType, ((FuncDef)elfNode).Name, ((FuncDef)elfNode).Args.ToArray(), false) 
        {
            FuncDef = (FuncDef)elfNode;
        }

        protected override string DumpContent()
        {
            var compiled = Body.Select(evi => evi.ToString()).StringJoin(Environment.NewLine).InjectLineNumbers0();
            return String.Format("<source code>{0}{1}{0}{0}<disassembly>{0}{2}",
                Environment.NewLine,
                FuncDef.SourceMethod == null ? "N/A" : FuncDef.SourceMethod.InjectLineNumbers1(),
                compiled);
        }
    }
}