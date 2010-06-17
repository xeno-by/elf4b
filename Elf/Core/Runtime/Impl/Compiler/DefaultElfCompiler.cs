using System;
using System.Collections.Generic;
using Elf.Core.Assembler;
using Elf.Core.Assembler.Literals;
using Elf.Exceptions.Compiler;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Expressions;
using Elf.Syntax.Ast.Statements;
using System.Linq;
using Elf.Syntax.Grammar;
using Elf.Helpers;

namespace Elf.Core.Runtime.Impl.Compiler
{
    public class DefaultElfCompiler : IElfCompiler
    {
        public VirtualMachine VM { get; private set; }
        private FuncDef Func { get; set; }

        public void Bind(VirtualMachine vm)
        {
            VM = vm;
        }
        
        public ElfVmInstruction[] Compile(FuncDef func)
        {
            /* hello bad code */ Func = func;
            return CompileImpl(func).ToArray();
        }

        public IEnumerable<ElfVmInstruction> CompileImpl(FuncDef func)
        {
            foreach (var evi in Compile(func.Body))
                yield return evi;

            // todo. implement stack non-corruption verification
            yield return new PopAll().BindToAstNode(func);
            yield return new Ret().BindToAstNode(func);
        }

        private IEnumerable<ElfVmInstruction> Compile(AstNode node)
        {
            try
            {
                if (node == null)
                {
                    return new ElfVmInstruction[0];
                }
                else
                {
                    IEnumerable<ElfVmInstruction> compiledImpl;
                    switch (node.NodeType)
                    {
                        case AstNodeType.Block:
                            compiledImpl = CompileBlock((Block)node);
                            break;

                        case AstNodeType.EmptyStatement:
                            compiledImpl = new ElfVmInstruction[0];
                            break;

                        case AstNodeType.ExpressionStatement:
                            compiledImpl = CompileExpression((ExpressionStatement)node);
                            break;

                        case AstNodeType.VarStatement:
                            compiledImpl = CompileVar((VarStatement)node);
                            break;

                        case AstNodeType.IfStatement:
                            compiledImpl = CompileIf((IfStatement)node);
                            break;

                        case AstNodeType.ReturnStatement:
                            compiledImpl = CompileReturn((ReturnStatement)node);
                            break;

                        case AstNodeType.LiteralExpression:
                            compiledImpl = CompileLiteral((LiteralExpression)node);
                            break;

                        case AstNodeType.VariableExpression:
                            compiledImpl = CompileVariable((VariableExpression)node);
                            break;

                        case AstNodeType.AssignmentExpression:
                            compiledImpl = CompileAssignment((AssignmentExpression)node);
                            break;

                        case AstNodeType.InvocationExpression:
                            compiledImpl = CompileInvocation((InvocationExpression)node);
                            break;

                        default:
                            throw new NotSupportedException(node.ToString());
                    }

                    var compiled = compiledImpl.ToArray();
                    compiled.ForEach(evi => evi.BindToAstNode(node));
                    return compiled;
                }
            }
            catch (Exception e)
            {
                if (e is UnexpectedCompilerException) throw;
                throw new UnexpectedCompilerException(Func, node, e);
            }
        }

        private IEnumerable<ElfVmInstruction> CompileBlock(Block b)
        {
            yield return new Enter();

            foreach (var stmt in b.Statements)
                foreach (var evi in Compile(stmt))
                    yield return evi;

            yield return new Leave();
        }

        private IEnumerable<ElfVmInstruction> CompileExpression(ExpressionStatement es)
        {
            // todo. would be nice to somehow guard ourselves against useless expressions, but how?
            // so far any expression can be meaningful because of side-effects

            foreach (var evi in Compile(es.Expression))
                yield return evi;

            // every expression results in a value on the stack
            // when we abandon the expression, we should get rid of that value
            yield return new Pop();
        }

        private IEnumerable<ElfVmInstruction> CompileVar(VarStatement vs)
        {
            yield return new Decl(vs.Name);
        }

        private int ifCounter;

        private IEnumerable<ElfVmInstruction> CompileIf(IfStatement @if)
        {
            var trueLabel = "if" + ifCounter + "t";
            var falseLabel = "if" + ifCounter++ + "f";

            foreach (var evi in Compile(@if.Test))
                yield return evi;
            yield return new Jf(falseLabel);

            yield return new Label(trueLabel);
            foreach (var evi in Compile(@if.Then))
                yield return evi;

            yield return new Label(falseLabel);
            foreach (var evi in Compile(@if.Else))
                yield return evi;
        }

        private IEnumerable<ElfVmInstruction> CompileReturn(ReturnStatement rs)
        {
            // todo. implement stack non-corruption verification
            yield return new PopAll();

            foreach (var evi in Compile(rs.Expression))
                yield return evi;

            yield return new Ret();
        }

        private IEnumerable<ElfVmInstruction> CompileLiteral(LiteralExpression le)
        {
            if (le.Token.Type == ElfParser.DecimalLiteral)
            {
                yield return new PushVal(new ElfNumberLiteral(le.Data));
            }
            else if (le.Token.Type == ElfParser.StringLiteral)
            {
                // very important: unquote the string literal
                yield return new PushVal(new ElfStringLiteral(le.Data.Unquote()));
            }
            else
            {
                throw new NotSupportedException(le.Token.Type.GetSymbolicName<ElfParser>());
            }
        }

        private IEnumerable<ElfVmInstruction> CompileVariable(VariableExpression ve)
        {
            yield return new PushRef(ve.Name);
        }

        private IEnumerable<ElfVmInstruction> CompileAssignment(AssignmentExpression ae)
        {
            foreach(var evi in Compile(ae.Expression))
                yield return evi;

            yield return new Dup();
            yield return new PopRef(ae.Target.Name);
        }

        private IEnumerable<ElfVmInstruction> CompileInvocation(InvocationExpression ie)
        {
            foreach(var arg in ie.Args)
                foreach(var evi in Compile(arg))
                    yield return evi;

            yield return new Invoke(ie.Name, ie.Args.Count());
        }
    }
}