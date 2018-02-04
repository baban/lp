using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Array : AstNode
    {
        ParseTreeNodeList nodes = null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count > 2)
            {
                for (int i = 1; i < nodes.Count()-1; i++)
                {
                    AddChild("Node", nodes[i]);
                }

            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string result = "";
            if (ChildNodes.Count() > 0)
            {
                result = ChildNodes.Select((node) => node.Evaluate(thread).ToString()).Aggregate((a, b) => a + ", " + b).ToString();
            }
            thread.CurrentNode = Parent;
            return "[" + result + "]";
        }
    }
}
