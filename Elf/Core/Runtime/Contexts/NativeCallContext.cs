using System;
using System.Collections.Generic;
using System.Text;
using Elf.Core.TypeSystem;
using Elf.Core.Reflection;
using System.Linq;
using Elf.Exceptions.Runtime;
using Elf.Helpers;

namespace Elf.Core.Runtime.Contexts
{
    public class NativeCallContext
    {
        public Stack<IElfObject> Stack { get; set; }
        public Stack<Scope> Scopes { get; set; }

        public NativeMethod Source { get; set; }

        private int _currentEvi;
        public int PrevEvi { get; private set; }
        public int CurrentEvi
        {
            get { return _currentEvi; }
            set
            {
                PrevEvi = _currentEvi;
                _currentEvi = value;
            }
        }

        public NativeCallContext(NativeMethod source, IElfObject @this, params IElfObject[] args) 
        {
            Stack = new Stack<IElfObject>();

            var callScope = new Scope();
            callScope.Add("@this", @this);
            source.FuncDef.Args.Zip(args, callScope.Add);
            Scopes = new Stack<Scope>();
            Scopes.Push(callScope);

            Source = source;
            if (source.FuncDef.Args.Count() != args.Length)
            {
                throw new UnexpectedElfRuntimeException(@this.VM, String.Format(
                   "Fatal error invoking native call '{0}({1})' with args '{2}'. Reason: args count mismatch.",
                   Source.Name, Source.FuncDef.Args.StringJoin(), args.StringJoin()));
            }

            CurrentEvi = 0;
            PrevEvi = -1;
            if (source.Body.IsNullOrEmpty())
            {
                throw new UnexpectedElfRuntimeException(@this.VM, String.Format(
                   "Fatal error invoking native call '{0}'. Reason: empty method body.", Source.Name));
            } 
        }

        public String Dump()
        {
            var sb = new StringBuilder();

            var args = Scopes.Count == 0 ? null : Scopes.Last();
            sb.AppendFormat("{0}({1}), current = {2} ({3})",
                Source.Name, args == null ? Source.Args.StringJoin() :
                    ("this = " + args["@this"] + (args.Count > 1 ? ", " : "") +
                     Source.Args.Select(a => a + " = " + args[a]).StringJoin()),
                CurrentEvi, Source.Body[CurrentEvi]).AppendLine();

            sb.AppendFormat("  stack = {0}", Stack.StringJoin()).AppendLine();
            sb.AppendFormat("  scopes = ");
            if (Scopes.Count > 1) sb.AppendLine().Append(
                Scopes.Reverse().Skip(1).Reverse().Select(s => s.Dump().Indent(2)).StringJoin(Environment.NewLine));
            if (Scopes.Count >= 1) sb.AppendLine().AppendLine("<call context>".Indent(2));

            return sb.ToString();
        }
    }
}