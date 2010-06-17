using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Helpers;

namespace Elf.Syntax.Ast.Defs
{
    public class ClassDef : AstNode
    {
        public String Name { get; private set; }
        public String Rtimpl { get; private set; }
        public IEnumerable<FuncDef> Funcs { get { return Children.Cast<FuncDef>(); } }

        public ClassDef(String name, String rtimpl, IEnumerable<FuncDef> funcs) 
            : base(AstNodeType.ClassDef, funcs.Cast<AstNode>())
        {
            Name = name;
            Rtimpl = rtimpl;
        }

        protected override string GetTPathNode() { return "c:" + Name; }
        protected override string GetTPathSuffix(int childIndex) { return null; }

        protected override string GetContent()
        {
            return String.Format("def {0} rtimpl {1}{3}{2}end", 
                Name, Rtimpl, Funcs.Select(f => f.Content).StringJoin(Environment.NewLine), Environment.NewLine);
        }
    }
}