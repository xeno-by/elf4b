using System;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Elf.Exceptions.Abstract;
using Elf.Helpers;
using Elf.Syntax.Ast;

namespace Elf.Exceptions.Parser
{
    public class SyntaxErrorException : ErroneousScriptException
    {
        public String Input { get; private set; }
        public int LineNumber { get { return AntlrException.Line; } }
        public int CharPositionInLine { get { return AntlrException.CharPositionInLine; } }
        public String AntlrMessage { get; private set; }
        public RecognitionException AntlrException { get { return (RecognitionException)InnerException; } }

        public SyntaxErrorException(string input, String antlrMessage, RecognitionException exception)
            : base(ElfExceptionType.SyntaxError, String.Empty, exception)
        {
            Input = input;
            AntlrMessage = antlrMessage;
        }

        public override string Message
        {
            get
            {
                var span = AntlrException.Token == null ? -1 : AntlrException.Token.Text.Length;
                var prettyInput = Input.InjectErrorMarker(LineNumber, CharPositionInLine, span).InjectLineNumbers1();

                return String.Format(
                    "Error parsing script:{6}{0}{6}{6}"+
                    "Reason: recognition exception '{1}' occurred at {2}:{3} (index is {4}). "+
                    "ANTLR reported: {5}.",
                    prettyInput, InnerException, LineNumber, CharPositionInLine, AntlrException.Index, AntlrMessage,
                    Environment.NewLine);
            }
        }

        public override string SourceCode
        {
            get { return Input; }
        }

        public override Span ErrorSpan
        {
            get
            {
                var abs = Input.GetAbsoluteIndex(LineNumber, CharPositionInLine);
                if (abs == -1)
                {
                    return Span.Invalid;
                }
                else
                {
                    var span = AntlrException.Token == null ? 0 : AntlrException.Token.Text.Length;
                    return Span.FromLength(abs, span);
                }
            }
        }

        public override CommonTree AntlrNode
        {
            get { return null; }
        }

        public override AstNode ElfNode
        {
            get { return null; }
        }
    }
}