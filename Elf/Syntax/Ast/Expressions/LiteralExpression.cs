using System;
using System.Linq;
using Antlr.Runtime;
using Elf.Helpers;
using Elf.Syntax.Grammar;

namespace Elf.Syntax.Ast.Expressions
{
    public class LiteralExpression : Expression
    {
        public IToken Token { get; private set; }
        public String Data { get; private set;}

        public LiteralExpression(String data)
            : base(AstNodeType.LiteralExpression, Enumerable.Empty<AstNode>())
        {
            Data = data;
        }

        public LiteralExpression(IToken token)
            : this(token.Text)
        {
            Token = token;
        }

        protected override string GetTPathNode() { return Token == null ? Data :
            String.Format("({0}, {1})", Data, Token.Type.GetSymbolicName<ElfParser>()); }
        protected override string GetTPathSuffix(int childIndex) { return null; }
        protected override string GetContent() { return Data; }
    }
}