using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Elf.Core.ClrIntegration;
using Elf.Core.Reflection;
using System.Linq;
using Elf.Core.TypeSystem;
using Elf.Exceptions.Loader;
using Elf.Helpers;

namespace Elf.Core.Runtime.Impl.Loaders
{
    public class ClrIntegrationLoader
    {
        public VirtualMachine VM { get; private set; }

        public ClrIntegrationLoader(VirtualMachine vm) 
        {
            VM = vm;
        }
        
        public void Load()
        {
            try
            {
                var classes = new Dictionary<String, ElfClass>();
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                    .Where(asm => asm.IsDefined(typeof(ElfDiscoverableAttribute), false)))
                {
                    // n0te. this is useful for debugging so I didn't remove it
//                    var aname = String.Format("{0} at {1}", assembly.FullName, assembly.Location);
//                    Trace.WriteLine("======= asm: " + aname + "=======");
//                    var oldKeys = classes.Keys.ToArray();

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsRtimpl())
                        {
                            if (type.IsOpenGeneric())
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error loading RTIMPL type '{0}'. Reason: type is an open generic.", type));
                            }

                            // so far we assume a single global namespace for class names
                            var @class = new ElfClass(null, type.RtimplOf(), type);

                            // todo. before release we can add russian-names-only regex validator here
                            if (@class.Name.IsNullOrEmpty())
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error loading RTIMPL type '{0}'. Reason: Elf class name is null or empty.", type));
                            }

                            if (classes.ContainsKey(@class.Name))
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error loading RTIMPL type '{0}'. Reason: duplicate Elf class name.", @class.Name));
                            }
                            else
                            {
                                classes.Add(@class.Name, @class);
                            }

                            foreach (var ctor in type.GetConstructors(
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(ctor1 => ctor1.IsRtimpl()))
                            {
                                @class.Ctors.Add(new ClrMethod(null, @class, @class.Name, ctor));
                            }

                            foreach (var method in type.GetMethods(
                                BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy |
                                BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(method1 => method1.IsRtimpl()))
                            {
                                @class.Methods.Add(new ClrMethod(null, @class, method.RtimplOf(), method));
                            }

                            VM.Classes.Add(@class);
                        }

                        if (type.IsElfSerializable())
                        {
                            var serializableAs = type.ElfSerializableAs();
                            if (serializableAs.IsNullOrEmpty())
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error registering type '{0}' as serializable. Reason: Type token is null or empty.", type));
                            }

                            if (!type.GetInterfaces().Any(iface => iface == typeof(IElfObject)))
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error registering type '{0}' as serializable. Reason: Type doesn't implement the IElfObject interface.", type));
                            }

                            var deserializer = type.ElfDeserializer();
                            if (deserializer == null)
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error registering type '{0}' as serializable. Reason: Type doesn't expose a static Parse method, neither it is convertible from string.", type));
                            }

                            VM.Deserializers.Add(serializableAs, deserializer);
                        }

                        if (type.IsRthelper())
                        {
                            foreach (var ctor in type.GetConstructors(
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(ctor1 => ctor1.IsRtimpl()))
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error registering type '{0}' as RTHELPER. Reason: Rthelpers cannot define RTIMPL constructors.", type));
                            }

                            foreach (var ctor in type.GetMethods(
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(ctor1 => ctor1.IsRtimpl()))
                            {
                                throw new UnexpectedLoaderException(String.Format(
                                    "Fatal error registering type '{0}' as RTHELPER. Reason: Rthelpers cannot define RTIMPL instance methods.", type));
                            }

                            foreach (var method in type.GetMethods(
                                BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic)
                                .Where(method1 => method1.IsRtimpl()))
                            {
                                VM.HelperMethods.Add(new ClrMethod(method.RtimplOf(), method));
                            }
                        }
                    }

                    // n0te. this is useful for debugging so I didn't remove it
//                    classes.Keys.Except(oldKeys).ForEach(cls => Trace.WriteLine(cls + " " + classes[cls]));
                }
            }
            catch (Exception e)
            {
                if (e is UnexpectedLoaderException) throw;
                throw new UnexpectedLoaderException(e);
            }
        }
    }
}