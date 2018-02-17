using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Hash : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count > 2)
            {
                AddChild("Node", nodes[1]);
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string result = "";
            if (ChildNodes.Count() > 0)
            {
                result = ChildNodes.First().Evaluate(thread).ToString();
            }
            thread.CurrentNode = Parent;
            return "{ " + result + " }";
        }
    }
}
