using System;
using Elf.Syntax.Ast.Expressions;

namespace Esath.Pie.AstRendering
{
    public interface IElfExpressionRenderer
    {
        Expression Target { get; }
        IRendererContext Ctx { get; }

        String RenderElfCode();
        String RenderPublicText();
    }

    public interface IElfExpressionRenderer<T> : IElfExpressionRenderer
        where T : Expression
    {
        new T Target { get; }
    }
}