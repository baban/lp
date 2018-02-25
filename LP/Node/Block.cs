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
            //ScopeInfo info = new ScopeInfo(this, false);
            //Scope scope = new Scope(info, null, thread.CurrentScope, new object[]{ });
            Object.LpObject result;
            if (ChildNodes.Count() > 0)
            {
                var assoc = ChildNodes.First();
                result = Object.LpBlock.initialize(ChildNodes);
            }
            else
            {
                result = Object.LpBlock.initialize();
            }
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
