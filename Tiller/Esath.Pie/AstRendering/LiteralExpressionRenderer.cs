using System.Threading;
using Elf.Syntax.Ast.Expressions;
using Esath.Data;
using Esath.Pie.Helpers;
using Esath.Data.Core;
using Elf.Helpers;

namespace Esath.Pie.AstRendering
{
    public class LiteralExpressionRenderer : IElfExpressionRenderer<LiteralExpression>
    {
        Expression IElfExpressionRenderer.Target { get { return Target; } }
        public LiteralExpression Target { get; private set; }
        public IRendererContext Ctx { get; private set; }

        public LiteralExpressionRenderer(LiteralExpression adaptee, IRendererContext ctx)
        {
            Target = adaptee;
            Ctx = ctx;
        }

        public string RenderElfCode()
        {
            var data = Target.Data;
            if (Target.Token == null) data = data.Quote();
            return data;
        }

        public string RenderPublicText()
        {
            var esath = Target.AsEsathObject();
            var publict = esath.ToUIString(Thread.CurrentThread.CurrentUICulture);
            if (esath is EsathText || esath is EsathString) publict = publict.Quote();
            return publict;
        }
    }
}