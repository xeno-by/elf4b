using System;
using System.Linq;
using Elf.Syntax.Ast.Expressions;
using Elf.Helpers;

namespace Esath.Pie.AstRendering
{
    public class InvocationExpressionRenderer : IElfExpressionRenderer<InvocationExpression>
    {
        Expression IElfExpressionRenderer.Target { get { return Target; } }
        public InvocationExpression Target { get; private set; }
        public IRendererContext Ctx { get; private set; }

        public InvocationExpressionRenderer(InvocationExpression adaptee, IRendererContext ctx)
        {
            Target = adaptee;
            Ctx = ctx;
        }

        private string Render(Func<IElfExpressionRenderer, string> fragRenderer)
        {
            // todo. check whether _ fits here as well
            // todo. find out better way to check: e.g. by using token codes
            if (!Char.IsLetterOrDigit(Target.Name[0]) && Target.Args.Count() == 2)
            {
                var firstArg = Ctx.CreateRenderer(Target.Args.ElementAt(0));
                var secondArg = Ctx.CreateRenderer(Target.Args.ElementAt(1));
                return String.Format("({0} {1} {2})",
                     fragRenderer(firstArg), Target.Name, fragRenderer(secondArg));
            }
            else
            {
                return String.Format("{0}({1})", Target.Name,
                     Target.Args.Select(arg => fragRenderer(Ctx.CreateRenderer(arg))).StringJoin());
            }
        }

        public string RenderElfCode()
        {
            return Render(a => a.RenderElfCode());
        }

        public string RenderPublicText()
        {
            return Render(a => a.RenderPublicText());
        }
    }
}