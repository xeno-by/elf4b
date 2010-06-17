using System;
using Elf.Syntax.Ast.Expressions;

namespace Esath.Pie.AstRendering
{
    public class AssignmentExpressionRenderer : IElfExpressionRenderer<AssignmentExpression>
    {
        Expression IElfExpressionRenderer.Target { get { return Target; } }
        public AssignmentExpression Target { get; private set; }
        public IRendererContext Ctx { get; private set; }

        public AssignmentExpressionRenderer(AssignmentExpression target, IRendererContext ctx)
        {
            Target = target;
            Ctx = ctx;
        }

        private string Render(Func<IElfExpressionRenderer, string> fragRenderer)
        {
            var firstArg = Ctx.CreateRenderer(Target.Target);
            var secondArg = Ctx.CreateRenderer(Target.Expression);
            return String.Format("{0} {1} {2}", fragRenderer(firstArg), "=", fragRenderer(secondArg));
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