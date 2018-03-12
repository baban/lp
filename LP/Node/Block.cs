using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Block : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count() > 0)
            {
                AddChild("Node", nodes[0]);
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject result = null;
            /*
            if (ChildNodes.Count() > 0)
            {
                result = Object.LpBlock.initialize(ChildNodes);
            }
            else
            {
                result = Object.LpBlock.initialize();
            }
            */
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
