using System.Collections;
using System.Collections.Generic;
using Elf.Core.Assembler;
using Elf.Core.Reflection;
using Elf.Core.TypeSystem;

namespace Elf.Core.Runtime.Impl
{
    public class DefaultEntryPoint : IEntryPoint
    {
        public VirtualMachine VM { get; private set; }
        public NativeMethod CodePoint { get; private set; }
        public IElfObject This { get; private set; }
        public IElfObject[] Args { get; private set; }

        public DefaultEntryPoint(NativeMethod codePoint, IElfObject @this, IElfObject[] args)
        {
            CodePoint = codePoint;
            This = @this;
            Args = args;
        }

        public void Bind(VirtualMachine vm)
        {
            VM = vm;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ElfVmInstruction> GetEnumerator()
        {
            var thread = new DefaultElfThread();
            thread.Bind(VM);
            thread.Startup(this);
            return thread;
        }
    }
}