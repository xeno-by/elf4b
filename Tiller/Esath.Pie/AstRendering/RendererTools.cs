using System;
using System.Linq;
using Elf.Helpers;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Expressions;
using Elf.Syntax.Ast.Statements;
using Elf.Syntax.AstBuilders;
using Elf.Syntax.Light;

namespace Esath.Pie.AstRendering
{
    public static class RendererTools
    {
        public static IElfExpressionRenderer CreateRenderer(this IRendererContext ctx, Expression target)
        {
            if (target is LiteralExpression)
            {
                return new LiteralExpressionRenderer((LiteralExpression)target, ctx);
            }
            else if (target is VariableExpression)
            {
                return new VariableExpressionRenderer((VariableExpression)target, ctx);
            }
            else if (target is InvocationExpression)
            {
                return new InvocationExpressionRenderer((InvocationExpression)target, ctx);
            }
            else if (target is AssignmentExpression)
            {
                return new AssignmentExpressionRenderer((AssignmentExpression)target, ctx);
            }
            else
            {
                throw new NotSupportedException(target.GetType().ToString());
            }
        }

        public static String RenderElfCode(this Expression target, IRendererContext ctx)
        {
            return ctx.CreateRenderer(target).RenderElfCode();
        }

        public static String RenderPublicText(this Expression target, IRendererContext ctx)
        {
            return ctx.CreateRenderer(target).RenderPublicText();
        }

        public static String RenderElfCode(this Script script, IRendererContext ctx)
        {
            var body = script.Classes.Single().Funcs.Single().Body;
            var lines = body.Children.Cast<ExpressionStatement>().Select(es => es.Expression).ToArray(); ;
            return lines.Select(l => l.RenderElfCode(ctx)).StringJoin(Environment.NewLine);
        }

        public static String RenderPublicText(this Script script, IRendererContext ctx)
        {
            var body = script.Classes.Single().Funcs.Single().Body;
            var lines = body.Children.Cast<ExpressionStatement>().Select(es => es.Expression).ToArray(); ;
            return lines.Select(l => l.RenderPublicText(ctx)).StringJoin(Environment.NewLine);
        }

        public static String RenderLightElfAsPublicText(this String light, IRendererContext ctx)
        {
            return RenderCanonicalElfAsPublicText(light.ToCanonicalElf(), ctx);
        }

        public static String RenderCanonicalElfAsPublicText(this String canonical, IRendererContext ctx)
        {
            var script = (Script)new ElfAstBuilder(canonical).BuildAstAllowLoopholes();
            return script.RenderPublicText(ctx);
        }
    }
}