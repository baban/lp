using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class MethodCall : AstNode
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Node = AddChild("Node", nodes[0]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            //Object.LpObject result = (Object.LpObject)Node.Evaluate(thread);
            thread.CurrentNode = Parent;
            //return result;
            return null;
        }
    }
}
