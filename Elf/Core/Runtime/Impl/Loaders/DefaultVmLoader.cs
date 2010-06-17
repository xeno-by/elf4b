using System;
using Elf.Core.Reflection;
using Elf.Exceptions;
using Elf.Exceptions.Loader;
using Elf.Syntax.Ast.Defs;
using System.Linq;

namespace Elf.Core.Runtime.Impl.Loaders
{
    public class DefaultVmLoader : IVmLoader
    {
        public VirtualMachine VM { get; private set; }

        public void Bind(VirtualMachine vm)
        {
            VM = vm;

            VM.Classes.Clear();
            VM.Deserializers.Clear();
            VM.HelperMethods.Clear();

            new ClrIntegrationLoader(VM).Load();
        }

        public void Load(Script script)
        {
            try
            {
                var loaded = new ElfScriptLoader(VM, script).Load();
                foreach (var wannabe in loaded)
                {
                    if (VM.Classes.Any(c => c.Name == wannabe.Name))
                    {
                        throw new ErroneousScriptLoaderException(script, wannabe.AstNode, ElfExceptionType.DuplicateClassLoaded);
                    }
                    else
                    {
                        foreach (var rtimpl in wannabe.Rtimpl.Ctors)
                            wannabe.Ctors.Add(new ClrMethod(wannabe.AstNode, wannabe, wannabe.Name, rtimpl.Rtimpl));

                        foreach (var rtimpl in wannabe.Rtimpl.Methods)
                        {
                            if (wannabe.Methods.OfType<NativeMethod>().Any(m => m.Name == rtimpl.Name))
                            {
                                throw new ErroneousScriptLoaderException(script, wannabe.AstNode, ElfExceptionType.DuplicateFuncLoaded);
                            }
                            else
                            {
                                wannabe.Methods.Add(rtimpl);
                            }
                        }

                        VM.Classes.Add(wannabe);
                    }
                }
            }
            catch (Exception e)
            {
                if (e is ApplicationException) throw;
                throw new UnexpectedLoaderException(script, e);
            }
        }
    }
}