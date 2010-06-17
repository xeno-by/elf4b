using System;
using Elf.Exceptions.Abstract;
using Elf.Syntax.Ast.Defs;

namespace Elf.Exceptions.Loader
{
    public class UnexpectedLoaderException : UnexpectedElfException
    {
        public Script Script { get; private set; }
        public String Reason { get; private set; }

        public UnexpectedLoaderException(Exception innerException)
            : base(String.Empty, innerException)
        {
        }

        public UnexpectedLoaderException(String reason)
        {
            Reason = reason;
        }

        public UnexpectedLoaderException(Script script, Exception innerException)
            : base(String.Empty, innerException)
        {
            Script = script;
        }

        public UnexpectedLoaderException(Script script, String reason, Exception innerException)
            : base(String.Empty, innerException)
        {
            Script = script;
            Reason = reason;
        }

        public override string Message
        {
            get
            {
                var scriptStr = String.Format("script:{0}{1}{0}{0}", Environment.NewLine, Script);
                var reasonStr = Reason ?? InnerException.Message;

                return String.Format(
                    "Unexpected error loading {0}\r\n" + "Reason: '{1}'.",
                    Script == null ? "stuff" : scriptStr, reasonStr);
            }
        }
    }
}