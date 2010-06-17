using Elf.Cola.Analysis;
using Elf.Exceptions.Abstract;

namespace Elf.Cola.Facta
{
    public class ScriptIsErroneousFactum : Factum
    {
        public ColaNode Node { get; private set; }
        public ErroneousScriptException Exception { get; private set; }

        public ScriptIsErroneousFactum(ColaNode node, ErroneousScriptException exception)
            : base(Severity.Fatal, node) 
        {
            Node = node;
            Exception = exception;
        }
    }
}