using System;
using Elf.Core.Reflection;
using Elf.Helpers;

namespace Elf.Core.TypeSystem
{
    // used to model instances of classes defined in the script source code 
    // as instances of their RTIMPL class
    //
    // this class shouldn't be inherited since it's a wrapper ONLY for ElfClasses
    // that don't have corresponding IElfObjects defined in the .NET codebehind

    public class ElfScriptDefinedClassInstance : ElfObjectImpl
    {
        private ElfClass _type;
        public override ElfClass Type { get { return _type; } }

        protected override string TypeName { get { return Type.Name; } }
        public Object ClrObject { get; private set; }

        public ElfScriptDefinedClassInstance(ElfClass elfClass, Object clrInstance)
        {
            _type = elfClass;
            ClrObject = clrInstance;
        }

        public override void Bind(VirtualMachine vm)
        {
            base.Bind(vm);
            if (ClrObject is ElfObjectImpl) 
                ((ElfObjectImpl)ClrObject).Bind(vm);
        }

        public override string ToString()
        {
            return "<" + ClrObject.ToInvariantString() + ">";
        }
    }
}