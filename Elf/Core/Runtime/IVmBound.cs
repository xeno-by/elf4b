namespace Elf.Core.Runtime
{
    public interface IVmBound
    {
        VirtualMachine VM { get; }
        void Bind(VirtualMachine vm);
    }
}