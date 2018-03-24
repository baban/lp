using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class RightUnary : AstNode
    {
        public AstNode Node { get; private set; }
        public ParseTreeNode Op;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Op = nodes[0];
            Node = AddChild("Node", nodes[1]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var result = ((Object.LpObject)Node.Evaluate(thread)).funcall(Op.Token.Text, new Object.LpObject[] { }, null);

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
