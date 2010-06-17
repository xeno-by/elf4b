using System;
using Antlr.Runtime.Tree;
using Elf.Exceptions.Abstract;
using Elf.Helpers;
using Elf.Syntax.Ast;
using Elf.Syntax.Ast.Defs;

namespace Elf.Exceptions.Loader
{
    public class ErroneousScriptLoaderException : ErroneousScriptException
    {
        public Script Script { get; private set; }
        public AstNode Entity { get; private set; }

        public ErroneousScriptLoaderException(Script script, AstNode entity, ElfExceptionType reason)
            : base(reason)
        {
            Script = script;
            Entity = entity;
            Script.ToString();
        }

        public override string Message
        {
            get
            {
                return String.Format(
                    "Error loading script:{0}{1}{0}{0}" + 
                    "Could not load entity '{2}'. Reason: '{3}'.",
                    Environment.NewLine, Script, Entity.FullTPath, Type);
            }
        }

        public override string SourceCode
        {
            get { return Script.AntlrNode.GetSourceCode(); }
        }

        public override Span ErrorSpan
        {
            get { return Entity.AntlrNode.GetOwnSpan(); }
        }

        public override CommonTree AntlrNode
        {
            get { return Entity.AntlrNode; }
        }

        public override AstNode ElfNode
        {
            get { return Entity; }
        }
    }
}