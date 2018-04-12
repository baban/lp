using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Operand : AstNode
    {
        public ParseTreeNode Op { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Op = nodes[0];
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var op = Op.Token.Text;
            thread.CurrentNode = Parent;
            return op;
        }
    }
}
