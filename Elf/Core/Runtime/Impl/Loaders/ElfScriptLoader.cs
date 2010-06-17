using System;
using System.Collections.Generic;
using Elf.Core.Reflection;
using Elf.Exceptions;
using Elf.Exceptions.Loader;
using Elf.Syntax.Ast.Defs;
using System.Linq;

namespace Elf.Core.Runtime.Impl.Loaders
{
    public class ElfScriptLoader
    {
        public Script Script { get; private set; }
        public VirtualMachine VM { get; private set; }

        public ElfScriptLoader(VirtualMachine vm, Script script) 
        {
            VM = vm;
            Script = script;
        }

        public ElfClass[] Load()
        {
            try
            {
                return LoadScript(Script).ToArray();
            }
            catch(Exception e)
            {
                if (e is ErroneousScriptLoaderException) throw;
                throw new UnexpectedLoaderException(Script, e);
            }
        }

        private IEnumerable<ElfClass> LoadScript(Script scriptDef)
        {
            var classes = new Dictionary<String, ElfClass>();
            foreach(var classDef in scriptDef.Classes)
            {
                var @class = LoadClass(classDef);
                if (classes.ContainsKey(@class.Name))
                {
                    throw new ErroneousScriptLoaderException(Script, classDef, ElfExceptionType.DuplicateClassLoaded);
                }
                else
                {
                    classes.Add(@class.Name, @class);
                }
            }

            return classes.Values;
        }

        private ElfClass LoadClass(ClassDef classDef)
        {
            // so far we assume a single global namespace for class names
            var rtimpl = VM.Classes.SingleOrDefault(c => c.Name == classDef.Rtimpl);
            if (rtimpl == null)
            {
                var exception = new ErroneousScriptLoaderException(Script, classDef, ElfExceptionType.ClassRtimplNotFound);
                exception.ToString();
                throw exception;
            }

            var @class = new ElfClass(classDef, classDef.Name, rtimpl);

            var funcs = new Dictionary<String, NativeMethod>();
            foreach (var funcDef in classDef.Funcs)
            {
                var func = LoadNativeFunc(@class, funcDef);
                if (funcs.ContainsKey(func.Name))
                {
                    throw new ErroneousScriptLoaderException(Script, funcDef, ElfExceptionType.DuplicateFuncLoaded);
                }
                else
                {
                    funcs.Add(func.Name, func);
                }
            }

            @class.Methods.AddRange(funcs.Values.Cast<ElfMethod>());
            return @class;
        }

        private NativeMethod LoadNativeFunc(ElfClass @class, FuncDef funcDef)
        {
            var native = new NativeMethod(funcDef, @class);
            native.Body = VM.Compiler.Compile(native.FuncDef);
            return native;
        }
    }
}