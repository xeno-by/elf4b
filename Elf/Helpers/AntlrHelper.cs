using System;
using System.Collections.Generic;
using System.Reflection;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.Linq;

namespace Elf.Helpers
{
    public static class AntlrHelper
    {
        public static CommonTree RecursivelyReplaceNullListsWithEmptyOnes(this CommonTree astNode)
        {
            if (astNode.Children == null)
            {
                var children = typeof(CommonTree).GetField("children", BindingFlags.Instance | BindingFlags.NonPublic);
                children.SetValue(astNode, new List<CommonTree>());
            }

            foreach (CommonTree child in astNode.Children)
            {
                child.RecursivelyReplaceNullListsWithEmptyOnes();
            }

            return astNode;
        }

        public static CommonTree XParent(this CommonTree astNode)
        {
            return (CommonTree)astNode.Parent;
        }

        public static CommonTree[] XChildren(this CommonTree astNode)
        {
            return astNode.XChildrenImpl().ToArray();
        }

        private static IEnumerable<CommonTree> XChildrenImpl(this CommonTree astNode)
        {
            foreach (CommonTree child in astNode.Children)
            {
                yield return child;
            }
        }

        public static String GetSymbolicName<T>(this int token)
            where T : Parser
        {
            var tokenTypes = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(fi => fi.IsLiteral && fi.FieldType == typeof(int))
                .ToLookup(fi => (int)fi.GetValue(null));

            return tokenTypes.Contains(token) ? tokenTypes[token].Single().Name : null;
        }

        public static String GetSourceCode(this CommonTree node)
        {
            var nonImaginary = node.GetFirstChildWithSourceCode();
            if (nonImaginary != null)
            {
                var input = ((CommonToken)nonImaginary.Token).InputStream;
                if (input is ANTLRStringStream)
                {
                    return new String((Char[])typeof(ANTLRStringStream).GetField("data",
                        BindingFlags.NonPublic | BindingFlags.Instance).GetValue(input));
                }
                else
                {
                    throw new NotSupportedException(String.Format(
                        "Unsupported ANTLR stream: '{0}'", input == null ? "null" : input.GetType().ToString()));
                }
            }
            else
            {
                return null;
            }
        }

        private static CommonTree GetFirstChildWithSourceCode(this CommonTree node)
        {
            if (node == null) return null;
            if (node.Token != null && ((CommonToken)node.Token).InputStream != null) return node;
            return node.XChildren().Count() == 0 ? null : node.XChildren()[0].GetFirstChildWithSourceCode();
        }

        public static Span GetOwnSpan(this CommonTree node)
        {
            var real = node.GetFirstNonImaginaryChild();
            if (real != null)
            {
                var source = real.GetSourceCode();
                return Span.FromLength(
                    source.GetAbsoluteIndex(real.Line, real.CharPositionInLine),
                    real.Token.Text.Length);
            }
            else
            {
                return Span.Invalid;
            }
        }

        public static CommonTree GetFirstNonImaginaryChild(this CommonTree node)
        {
            if (node == null) return null;
            if (node.Token != null && node.Token.Line > 0 && node.Token.CharPositionInLine > 1) return node;
            return node.XChildren().Count() == 0 ? null : node.XChildren()[0].GetFirstNonImaginaryChild();
        }
    }
}
