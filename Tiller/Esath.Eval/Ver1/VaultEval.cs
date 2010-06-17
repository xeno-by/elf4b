using System;
using System.Collections.Generic;
using DataVault.Core.Api;
using Elf.Exceptions;
using Elf.Exceptions.Runtime;
using Elf.Interactive;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Expressions;
using Elf.Syntax.Ast.Statements;
using Elf.Syntax.AstBuilders;
using Elf.Syntax.Light;
using System.Linq;
using Elf.Helpers;
using Esath.Pie.Helpers;
using Esath.Pie.AstRendering;

namespace Esath.Eval.Ver1
{
    public static class VaultEval
    {
        public static object Eval(this IBranch b, IVault repository)
        {
            try
            {
                AppDomain.CurrentDomain.Load("Esath.Data");
                var ei = new ElfInteractive();
                var stack = new List<Expression>();
                var nodes = new Dictionary<String, IBranch>();

                var expandedCode = ExpandRhs(b, repository, stack, nodes).RenderElfCode(null);
                nodes.ForEach(kvp => ei.Ctx.Add(kvp.Key, kvp.Value));

                return ei.Eval(expandedCode).Retval;
            }
            catch(BaseEvalException)
            {
                throw;
            }
            catch(ErroneousScriptRuntimeException esex)
            {
                if (esex.Type == ElfExceptionType.OperandsDontSuitMethod)
                {
                    throw new ArgsDontSuitTheFunctionException(esex.Thread.RuntimeContext.PendingClrCall, esex);
                }
                else
                {
                    throw new UnexpectedErrorException(esex);
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException is FormatException)
                {
                    throw new BadFormatOfSerializedStringException(ex);
                }
                else
                {
                    throw new UnexpectedErrorException(ex);
                }
            }
        }

        private static Expression ExpandRhs(IBranch b, IVault repository, List<Expression> stack, Dictionary<String, IBranch> nodes)
        {
            var elf = b.GetValue("elfCode").ContentString.ToCanonicalElf();
            var script = (Script)new ElfAstBuilder(elf).BuildAst();
            var assign = (AssignmentExpression)((ExpressionStatement)
                script.Classes.Single().Funcs.Single().Body.Statements.Single()).Expression;

            var rhs = assign.Expression;
            if (stack.Contains(rhs))
            {
                throw new EvalStackOverflowException();
            }
            else
            {
                stack.Add(rhs);
                return Expand(rhs, b.Vault, repository, stack, nodes);
            }
        }

        private static Expression Expand(Expression ex, IVault vault, IVault repository, List<Expression> stack, Dictionary<String, IBranch> nodes)
        {
            if (ex is VariableExpression)
            {
                return Expand((VariableExpression)ex, vault, repository, stack, nodes);
            }
            else
            {
                ex.Flatten(n => n.Children.Cast<Expression>()).OfType<VariableExpression>()
                    .ForEach(v => v.ReplaceMeWith(() => Expand(v, vault, repository, stack, nodes)));
                return ex;
            }
        }

        private static Expression Expand(VariableExpression vex, IVault vault, IVault repository, List<Expression> stack, Dictionary<String, IBranch> nodes)
        {
            var @ref = vault.GetBranch(vex.Name.FromElfIdentifier());
            if (@ref == null)
            {
                throw new ReferencedBranchDoesNotExistException(vex.Name);
            }

            if (@ref.IsFov())
            {
                if (@ref.GetOrCreateValue("declarationType", "source").ContentString == "source")
                {
                    String val;
                    if (repository == null)
                    {
                        val = @ref.GetValue("valueForTesting").ContentString;
                    }
                    else
                    {
                        var external = @ref.GetValue("repositoryValue").ContentString;
                        val = repository.GetValue(external).ContentString;
                    }

                    return new LiteralExpression(String.Format("[[{0}]]{1}", @ref.GetValue("type").ContentString, val));
                }
                else
                {
                    return ExpandRhs(@ref, repository, stack, nodes);
                }
            }
            else
            {
                var varName = @ref.VPath.ToElfIdentifier();
                nodes[varName] = @ref;
                return new VariableExpression(varName);
            }
        }
    }
}
