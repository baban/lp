using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
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
            return Object.LpHash.initialize();
            thread.CurrentNode = this;
            //string result = "";
            if (ChildNodes.Count() > 0)
            {
                //result = ChildNodes.First().Evaluate(thread);
            }
            thread.CurrentNode = Parent;
            return Object.LpHash.initialize();
        }
    }
}
