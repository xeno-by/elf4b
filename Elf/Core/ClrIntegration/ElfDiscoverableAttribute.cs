using System;

namespace Elf.Core.ClrIntegration
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class ElfDiscoverableAttribute : Attribute
    {
    }
}