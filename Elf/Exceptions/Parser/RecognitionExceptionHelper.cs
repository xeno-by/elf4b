using System;
using System.Reflection;
using Antlr.Runtime;

namespace Elf.Exceptions.Parser
{
    public static class RecognitionExceptionHelper
    {
        public static SyntaxErrorException Report(BaseRecognizer source, RecognitionException e)
        {
            var input = source.Input.ToString();
            if (source.Input is ANTLRStringStream)
                input = new String((Char[])typeof(ANTLRStringStream).GetField("data", 
                    BindingFlags.NonPublic | BindingFlags.Instance).GetValue(source.Input));

            var antlrMessage = source.GetErrorHeader(e) + " " + source.GetErrorMessage(e, source.TokenNames);
            throw new SyntaxErrorException(input, antlrMessage, e);
        }
    }
}