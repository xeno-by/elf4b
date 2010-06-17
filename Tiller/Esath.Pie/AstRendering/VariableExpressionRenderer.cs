using Elf.Syntax.Ast.Expressions;

namespace Esath.Pie.AstRendering
{
    public class VariableExpressionRenderer : IElfExpressionRenderer<VariableExpression>
    {
        Expression IElfExpressionRenderer.Target { get { return Target; } }
        public VariableExpression Target { get; private set; }
        public IRendererContext Ctx { get; private set; }

        public VariableExpressionRenderer(VariableExpression adaptee, IRendererContext ctx)
        {
            Target = adaptee;
            Ctx = ctx;
        }

        public string RenderElfCode()
        {
            return Target.Name;
        }

        public string RenderPublicText()
        {
            return Ctx.GetDisplayName(Target.Name);
        }
    }
}