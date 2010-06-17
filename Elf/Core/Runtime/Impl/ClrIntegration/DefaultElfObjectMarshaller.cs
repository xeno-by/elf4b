using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Elf.Core.Assembler.Literals;
using Elf.Core.TypeSystem;
using System.Linq;
using Elf.Helpers;

namespace Elf.Core.Runtime.Impl.ClrIntegration
{
    public class DefaultElfObjectMarshaller : IElfObjectMarshaller
    {
        public VirtualMachine VM { get; private set; }

        public void Bind(VirtualMachine vm)
        {
            VM = vm;
        }

        private Type[] DeclaringTypes { get { return VM.Classes.Select(c => c.ClrType).Distinct().ToArray(); } }
        private MethodInfo[] ImplicitCasts { get { return DeclaringTypes
            .Select(c => c.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.IsSpecialName && m.Name == "op_Implicit")
                .Where(m => m.DeclaringType == c)).Flatten().ToArray(); } }

        public object Marshal(IElfObject elf)
        {
            if (elf == null)
            {
                return null;
            }
            else
            {
                if (elf is ElfVoid)
                {
                    return new ElfVoid();
                }
                else if (elf is ElfScriptDefinedClassInstance)
                {
                    return ((ElfScriptDefinedClassInstance)elf).ClrObject;
                }
                else
                {
                    var cast = ImplicitCasts.Where(
                        mi => mi.GetParameters()[0].ParameterType.IsAssignableFrom(elf.GetType())).SingleOrDefault();
                    if (cast != null)
                    {
                        return cast.Invoke(null, new object[] { elf });
                    }
                    else
                    {
                        var valprop = elf.GetType().GetProperty("Val", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (valprop != null && valprop.CanRead)
                        {
                            return valprop.GetValue(elf, null);
                        }
                        else
                        {
                            throw new NotSupportedException(String.Format(
                                "Elf object '{0}' of type '{1}' is not supported.",
                                elf, elf.GetType()));
                        }
                    }
                }
            }
        }

        public IElfObject Unmarshal(object clr)
        {
            if (clr == null)
            {
                return null;
            }
            else
            {
                if (clr is IElfObject)
                {
                    ((IElfObject)clr).Bind(VM);
                    return (IElfObject)clr;
                }
                else if (clr is ElfLiteral)
                {
                    if (clr is ElfStringLiteral)
                    {
                        var val = ((ElfStringLiteral)clr).Val;
                        var match = Regex.Match(val, @"^\[\[(?<token>.*?)\]\](?<content>.*)$");
                        if (match.Success)
                        {
                            var deserializer = VM.Deserializers[match.Result("${token}")];

                            var elfObject = deserializer(match.Result("${content}"));
                            elfObject.Bind(VM);
                            return elfObject;
                        }
                        else
                        {
                            return Unmarshal(val);
                        }
                    }
                    else
                    {
                        return Unmarshal(clr.GetType().GetProperty("Val").GetValue(clr, null));
                    }
                }
                else
                {
                    var cast = ImplicitCasts.Where(
                        mi => mi.GetParameters()[0].ParameterType == clr.GetType()).SingleOrDefault();

                    if (cast != null)
                    {
                        var elfObject = (IElfObject)cast.Invoke(null, new object[] { clr });
                        elfObject.Bind(VM);
                        return elfObject;
                    }
                    else
                    {
                        var classes = VM.Classes.Where(c => c.Rtimpl == null);
                        var ctors = classes.ToDictionary(c => c, 
                            c => c.ClrType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            .Where(ctor => ctor.GetParameters().Length == 1)
                            .SingleOrDefaultIfMoreOrLess());

                        var suitable = ctors.SingleOrDefaultIfMoreOrLess(kvp => kvp.Value != null &&
                            kvp.Value.GetParameters()[0].ParameterType.IsAssignableFrom(clr.GetType())).Value;
                        if (suitable != null)
                        {
                            var elfObject = (IElfObject)suitable.Invoke(new object[] { clr });
                            elfObject.Bind(VM);
                            return elfObject;
                        }
                        else
                        {
                            throw new NotSupportedException(String.Format(
                                "Clr object '{0}' of type '{1}' is not supported.",
                                clr, clr.GetType()));
                        }
                    }
                }
            }
        }
    }
}