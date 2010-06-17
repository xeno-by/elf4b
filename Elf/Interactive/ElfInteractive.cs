using System;
using Elf.Core;
using Elf.Helpers;
using System.Linq;

namespace Elf.Interactive
{
    public class ElfInteractive
    {
        private VirtualMachine VM { get; set; }
        public PropertyBag Ctx
        {
            get { return (PropertyBag)VM.Context["iactx"]; }
            set { VM.Context["iactx"] = value; }
        }

        public ElfInteractive()
        {
            VM = new VirtualMachine();
            VM.Context.Add("iactx", new PropertyBag());
        }

        public void Load(String elfClassDef)
        {
            VM.Load(elfClassDef);
        }

        private String WrapInteractiveElf(String interactive)
        {
            var funcBody = interactive;
            if (funcBody.SelectLines().Length == 1 && !funcBody.Contains(";")) funcBody = "ret " + funcBody;

            var funcDef = String.Format("def Main(){0}{1}{0}end", 
                Environment.NewLine, funcBody.Indent(1));

            var className = ("Interactive_" + Guid.NewGuid()).Where(c => c != '-').ToArray();
            return String.Format("def {1} rtimpl ElfInteractiveScript{0}{2}{0}end", 
                Environment.NewLine, new String(className), funcDef.Indent(1));
        }

        public EvalResult Eval(String elfCode)
        {
            var wrapped = WrapInteractiveElf(elfCode);
            VM.Load(wrapped);

            var changeset = new ChangeSet(() => Ctx, v => { Ctx = v; }).StartRecording();
            Ctx = new PropertyBag(changeset.BaseLine);

            try
            {
                var className = wrapped.Substring(0, wrapped.NthIndexOf(" ", 2)).Substring(4);
                var retval = VM.CreateEntryPoint(className, "Main").RunTillEnd();
                return new EvalResult(retval, changeset.Capture());
            }
            finally
            {
                Ctx = new PropertyBag(changeset.BaseLine);
            }
        }
    }
}
