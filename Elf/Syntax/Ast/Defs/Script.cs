using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Helpers;

namespace Elf.Syntax.Ast.Defs
{
    public class Script : AstNode
    {
        public IEnumerable<ClassDef> Classes { get { return Children.Cast<ClassDef>(); } }

        public Script(IEnumerable<ClassDef> classes) 
            : base(AstNodeType.Script, classes.Cast<AstNode>()) 
        {
        }

        protected override string GetTPathNode() { return "s"; }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return Classes.Select(c => c.Content).StringJoin(Environment.NewLine + Environment.NewLine); }
    }
}