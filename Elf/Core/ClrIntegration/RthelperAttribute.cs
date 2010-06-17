using System;

namespace Elf.Core.ClrIntegration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RthelperAttribute : Attribute
    {
    }
}