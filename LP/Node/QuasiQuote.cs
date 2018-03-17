using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class QuasiQuote : AstNode
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var node = treeNode.GetMappedChildNodes().Last();
            Node = AddChild("Stmt", node);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject result = (Object.LpObject)Node.Evaluate(thread);
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
