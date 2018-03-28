using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class BlockArg : AstNode
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if(nodes.Count > 0)
            {
                Node = AddChild("Node", nodes[0]);
            } else
            {
                Node = null;
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            if (Node == null) return null;

            thread.CurrentNode = this;

            Object.LpObject result = (Object.LpObject)Node.Evaluate(thread);

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
