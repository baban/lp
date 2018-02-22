using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class BinExpr : AstNode
    {
        public AstNode Left { get; private set; }
        public ParseTreeNode Op;
        public AstNode Right { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Left = AddChild("Left", nodes[0]);
            Op = nodes[1];
            Right = AddChild("Right", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            Object.LpObject result = ((Object.LpObject)Left.Evaluate(thread)).funcall(Op.Token.Text, new Object.LpObject[] { (Object.LpObject)Right.Evaluate(thread) }, null);

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
