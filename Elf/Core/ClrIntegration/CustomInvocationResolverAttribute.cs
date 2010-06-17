using System;
using Elf.Core.Runtime;
using Elf.Exceptions.Loader;

namespace Elf.Core.ClrIntegration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomInvocationResolverAttribute : Attribute
    {
        public Type Type { get; private set; }

        public CustomInvocationResolverAttribute(Type type)
        {
            Type = type;
            if (!typeof(IInvocationResolver).IsAssignableFrom(type))
            {
                throw new UnexpectedLoaderException(String.Format(
                    "'{0}' is not a valid invocation resolver", type));
            }
        }
    }
}