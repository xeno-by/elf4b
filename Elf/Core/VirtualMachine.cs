using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Elf.Core.Runtime.Impl;
using Elf.Core.Runtime.Impl.Loaders;
using Elf.Core.Reflection;
using Elf.Core.Runtime;
using Elf.Core.Runtime.Impl.ClrIntegration;
using Elf.Core.Runtime.Impl.Compiler;
using Elf.Core.TypeSystem;
using Elf.Syntax.Ast.Defs;
using Elf.Helpers;
using System.Linq;
using Elf.Syntax.AstBuilders;

namespace Elf.Core
{
    public class VirtualMachine
    {
        public IVmLoader Loader { get; set; }
        public IElfCompiler Compiler { get; set; }
        public IElfObjectMarshaller Marshaller { get; set; }

        public List<ElfClass> Classes { get; private set; }
        public Dictionary<String, Func<String, IElfObject>> Deserializers { get; private set; }
        public List<ClrMethod> HelperMethods { get; private set; }

        public List<IElfThread> Threads { get; private set; }
        public IElfThread CurrentThread { get; set; }

        public Dictionary<String, Object> Context { get; private set; }

        public VirtualMachine()
        {
            Classes = new List<ElfClass>();
            Context = new Dictionary<String, Object>();
            Threads = new List<IElfThread>();

            Deserializers = new Dictionary<String, Func<String, IElfObject>>();
            HelperMethods = new List<ClrMethod>();

            Loader = new DefaultVmLoader();
            Loader.Bind(this);

            Compiler = new DefaultElfCompiler();
            Compiler.Bind(this);

            Marshaller = new DefaultElfObjectMarshaller();
            Marshaller.Bind(this);
        }

        public void Load(String elfCode)
        {
            Loader.Load((Script)new ElfAstBuilder(elfCode).BuildAst());
        }

        public String DumpAll()
        {
            var sb = new StringBuilder();
            sb.AppendLine("********** Loaded classes **********").AppendLine();
            sb.AppendLine(Classes.OrderBy(c => c.Name).Select(c => c.Dump()).StringJoin(Environment.NewLine));

            sb.AppendLine("********** Threads **********").AppendLine();
            sb.Append(Threads.Select(t => ((DefaultElfThread)t).Dump()).StringJoin(Environment.NewLine));
            if (Threads.Count == 0) sb.AppendLine("No threads detected.");

            return sb.ToString();
        }
    }
}
