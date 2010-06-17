using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Elf.Exceptions;
using Elf.Exceptions.Parser;
using Elf.Helpers;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Defs;
using Elf.Syntax.Ast.Expressions;
using Elf.Syntax.Ast.Statements;
using Elf.Syntax.Grammar;

namespace Elf.Syntax.AstBuilders
{
    public class ElfAstBuilder
    {
        public String ElfCode { get; private set; }
        private bool AllowLoopholes { get; set; }

        public ElfAstBuilder(String elfCode)
        {
            ElfCode = elfCode.Trim();
            RegisterParseRules();
        }

        public AstNode BuildAst()
        {
            return BuildAst(false);
        }

        public AstNode BuildAstAllowLoopholes()
        {
            return BuildAst(true);
        }

        private AstNode BuildAst(bool allowLoopholes)
        {
            AllowLoopholes = allowLoopholes;
            return ParseAntlrNode(AcquireAntlrAst()).Single();
        }

        // throws SyntaxErrorException
        private CommonTree AcquireAntlrAst()
        {
            var input = new ANTLRStringStream(ElfCode);
            var lex = new ElfLexer(input);
            var tokens = new CommonTokenStream(lex);
            var parser = new ElfParser(tokens);
            var esV3Ast = (CommonTree)parser.script().Tree;
            return esV3Ast.RecursivelyReplaceNullListsWithEmptyOnes();
        }

        private IEnumerable<AstNode> ParseAntlrNode(CommonTree node)
        {
            try
            {
                // throws SemanticErrorException, UnexpectedParserException
                var elfNodes = SelectRule(node)(node).ToArray();
                elfNodes.ForEach(elfNode => elfNode.BindToAntlrNode(node));
                return elfNodes;
            }
            catch (Exception e)
            {
                if (e is ApplicationException) throw;
                throw new UnexpectedParserException(ElfCode, node, e);
            }
        }

        private IEnumerable<AstNode> ParseImpl(IEnumerable nodes)
        {
            foreach (CommonTree node in nodes)
                foreach (var elfNode in ParseAntlrNode(node))
                    yield return elfNode;
        }

        private AstNode[] ParseAntlrNode(IEnumerable<CommonTree> nodes)
        {
            return ParseImpl(nodes).ToArray();
        }

        private AstNode[] ParseAntlrNode(IList nodes)
        {
            return ParseImpl(nodes).ToArray();
        }

        private Dictionary<Func<CommonTree, bool>, Func<CommonTree, IEnumerable<AstNode>>> _rules =
            new Dictionary<Func<CommonTree, bool>, Func<CommonTree, IEnumerable<AstNode>>>();

        private void AddRule(Func<CommonTree, IEnumerable<AstNode>> parser, params int[] tokenTypes)
        {
            _rules.Add(t => tokenTypes.Contains(t.Token.Type), parser);
        }

        private void RegisterParseRules()
        {
            AddRule(ParseScript, ElfParser.SCRIPT);
            AddRule(ParseClass, ElfParser.CLASS);
            AddRule(ParseFunc, ElfParser.FUNC);
            AddRule(ParseBlock, ElfParser.BLOCK);

            AddRule(ParseExpr, ElfParser.EXPR);
            AddRule(ParseVar, ElfParser.VAR);
            AddRule(ParseRet, ElfParser.RET);
            AddRule(ParseIf, ElfParser.IF);

            AddRule(ParseParexpr, ElfParser.PAREXPR);
            AddRule(ParseLiteral, ElfParser.DecimalLiteral, ElfParser.StringLiteral);
            AddRule(ParseVariable, ElfParser.Identifier);
            AddRule(ParseAssignment, ElfParser.ASSIGN);
            AddRule(ParseRegularInvocation, ElfParser.CALL, ElfParser.INDEX);
            AddRule(ParseOperatorInvocation,
                ElfParser.POS, ElfParser.NEG, 
                ElfParser.POW, ElfParser.MUL, ElfParser.DIV, ElfParser.ADD, ElfParser.SUB,
                ElfParser.LT, ElfParser.GT, ElfParser.LTE, ElfParser.GTE, ElfParser.EQ, ElfParser.NEQ,
                ElfParser.NOT, ElfParser.AND, ElfParser.OR);
        }

        private Func<CommonTree, IEnumerable<AstNode>> SelectRule(CommonTree node)
        {
            var applicableRule = _rules.Where(rule => rule.Key(node))
                .Select(rule => rule.Value).SingleOrDefault();
            if (applicableRule != null)
            {
                return applicableRule;
            }
            else
            {
                throw new NotSupportedException(String.Format(
                    "No idea how to parse the node '{0}'.", node));
            }
        }

        private IEnumerable<AstNode> ParseScript(CommonTree node)
        {
            yield return new Script(ParseAntlrNode(node.Children).Cast<ClassDef>());
        }

        private IEnumerable<AstNode> ParseClass(CommonTree node)
        {
            var decl = node.XChildren().Single(c => c.Token.Type == ElfParser.DECL);
            var name = decl.XChildren()[0].Text;
            var rtimpl = decl.XChildren()[1].XChildren().ElementAt(0).Text;

            yield return new ClassDef(name, rtimpl, ParseAntlrNode(node.XChildren().Skip(1)).Cast<FuncDef>());
        }

        private IEnumerable<AstNode> ParseFunc(CommonTree node)
        {
            var decl = node.XChildren().Single(c => c.Token.Type == ElfParser.DECL);
            var name = decl.XChildren()[0].Text;
            var args = decl.XChildren()[1].XChildren().Select(c => c.Text);
            var bodyBlock = node.XChildren()[1];

            yield return new FuncDef(name, args, (Block)ParseAntlrNode(bodyBlock).Single());
        }

        private IEnumerable<AstNode> ParseBlock(CommonTree node)
        {
            yield return new Block(ParseAntlrNode(node.Children).Cast<Statement>());
        }

        private IEnumerable<AstNode> ParseExpr(CommonTree node)
        {
            yield return new ExpressionStatement((Expression)ParseAntlrNode(node.XChildren()[0]).Single());
        }

        private IEnumerable<AstNode> ParseVar(CommonTree node)
        {
            foreach(var varDecl in node.XChildren())
            {
                if (varDecl.XChildren().Count() == 0)
                {
                    yield return new VarStatement(varDecl.Text);
                }
                else
                {
                    yield return new VarStatement(varDecl.XChildren()[0].Text);
                    yield return new ExpressionStatement(
                        (AssignmentExpression)ParseAntlrNode(varDecl).Single());
                }
            }
        }

        private IEnumerable<AstNode> ParseRet(CommonTree node)
        {
            if (node.XChildren().Count() == 0)
            {
                yield return new ReturnStatement();
            }
            else
            {
                yield return new ReturnStatement((Expression)ParseAntlrNode(node.XChildren()[0]).Single());
            }
        }

        private IEnumerable<AstNode> ParseIf(CommonTree node)
        {
            var test = (Expression)ParseAntlrNode(node.XChildren()[0]).Single();
            var then = (Block)ParseAntlrNode(node.XChildren()[1]).Single();

            if (node.XChildren().Count() == 2)
            {
                yield return new IfStatement(test, then);
            }
            else
            {
                var @else = (Block)ParseAntlrNode(node.XChildren()[2]).Single();
                yield return new IfStatement(test, then, @else);
            }
        }

        private IEnumerable<AstNode> ParseParexpr(CommonTree node)
        {
            yield return ParseAntlrNode(node.XChildren().ElementAt(0)).Single();
        }

        private IEnumerable<AstNode> ParseLiteral(CommonTree node)
        {
            CheckIfLoophole(node);
            yield return new LiteralExpression(node.Token);
        }

        private IEnumerable<AstNode> ParseVariable(CommonTree node)
        {
            CheckIfLoophole(node);
            yield return new VariableExpression(node.Text);
        }

        private IEnumerable<AstNode> ParseAssignment(CommonTree node)
        {
            var target = ParseAntlrNode(node.XChildren()[0]).Single();
            if (!(target is VariableExpression))
            {
                throw new SemanticErrorException(ElfCode, node, ElfExceptionType.InvalidAssignmentLhs);
            }

            var expression = (Expression)ParseAntlrNode(node.XChildren()[1]).Single();
            yield return new AssignmentExpression((VariableExpression)target, expression);
        }

        private IEnumerable<AstNode> ParseRegularInvocation(CommonTree node)
        {
            CheckIfLoophole(node);
            yield return new InvocationExpression(node.XChildren()[0].Text,
                ParseAntlrNode(node.XChildren()[1].Children).Cast<Expression>());
        }

        private IEnumerable<AstNode> ParseOperatorInvocation(CommonTree node) 
        {
            yield return new InvocationExpression(node.Text, ParseAntlrNode(node.Children).Cast<Expression>());
        }

        private void CheckIfLoophole(CommonTree node)
        {
            if (!AllowLoopholes && node.Text == "?")
            {
                throw new SemanticErrorException(ElfCode, node, ElfExceptionType.LoopholesAreNowDisallowed);
            }
        }
    }
}
