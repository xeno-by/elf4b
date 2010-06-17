using System;
using System.Collections.Generic;
using System.Linq;
using Elf.Cola.Parameters;
using Elf.Helpers;

namespace Elf.Cola
{
    public class ColaNode
    {
        public Guid Id { get; private set; }
        public String Name { get; private set; }
        public String Script { get; set; }
        public ParametersValues ParamValues { get; set; }

        public ColaNode Parent { get; private set; }
        public IEnumerable<ColaNode> Parents 
        {
            get
            {
                for (var current = this.Parent; current != null; current = current.Parent)
                    yield return current;
            } 
        }

        public int ChildIndex { get; private set; }
        public IEnumerable<ColaNode> Children { get; private set; }

        public ColaNode(String name, params ColaNode[] children)
            : this(name, (IEnumerable<ColaNode>)children)
        {
            
        }

        public ColaNode(String name, IEnumerable<ColaNode> children)
        {
            Id = Guid.NewGuid();
            Name = name;
            ParamValues = new ParametersValues();
            Children = new List<ColaNode>(children).AsReadOnly();

            var childIndex = 0;
            foreach (var child in Children)
            {
                child.Parent = this;
                child.ChildIndex = childIndex++;
            }
        }

        public String TPath
        {
            get
            {
                return (Parents.Reverse().Concat(this.AsArray()))
                    .Select(node => node.Name.IsNullOrEmpty() ? "??" : node.Name).StringJoin(".");
            }
        }

        public override string ToString()
        {
//            return String.Format("[{0} -> {1}]", TPath, 
//               Script.IsNullOrEmpty() ? "N/A" : 
//               String.Format("{0}{1}", Environment.NewLine, Script.InjectLineNumbers1()));
            return TPath;
        }
    }
}