using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Assoc : AstNode
    {
        public AstNode Pairs { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("Assoc", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var result = ChildNodes.Select((node) => node.Evaluate(thread).ToString()).Aggregate((a, b) => a + ", " + b).ToString();
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
