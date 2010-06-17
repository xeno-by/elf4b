using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Esath.Pie.Helpers
{
    public static class AstUtils
    {
        public static AstNode ReplaceMeWith(this AstNode old, AstNode @new)
        {
            return ReplaceMeWith(old, () => @new);
        }

        public static AstNode ReplaceMeWith(this AstNode old, Func<AstNode> newf)
        {
            var parset = typeof(AstNode).GetProperty("Parent").GetSetMethod(true);
            var chiset = typeof(AstNode).GetProperty("Children").GetSetMethod(true);
            var chiiset = typeof(AstNode).GetProperty("ChildIndex").GetSetMethod(true);

            var parent = old.Parent;
            var chiindex = old.ChildIndex;

            // if you uncomment this, something will crash, namely Extract fx for a direct child of a root expression
//            parset.Invoke(old, new object[] { null });
//            chiiset.Invoke(old, ((object)-1).AsArray());
            var @new = newf();

            var head = parent.Children.Take(chiindex);
            var tail = parent.Children.Skip(chiindex + 1);
            var newchi = new List<AstNode>(head.Concat(@new.AsArray()).Concat(tail)).AsReadOnly();

            chiset.Invoke(parent, newchi.AsArray());
            parset.Invoke(@new, parent.AsArray());
            chiiset.Invoke(@new, ((object)chiindex).AsArray());

            return @new;
        }
    }
}