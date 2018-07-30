using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class CaseStmt : AstNode
    {
        AstNode Node = null;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Node = AddChild("Node", nodes[0]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var scope = thread.CurrentScope;
            var slot = scope.Info.GetSlot("_case");
            var function = (Object.LpObject)scope.GetValue(slot.Index);

            Object.LpObject result = null;
            thread.CurrentNode = Parent;
            return result;
            /*
            thread.CurrentNode = this;
            string result = "if(" + Expr.Evaluate(thread).ToString() + ")" + Stmts.Evaluate(thread).ToString() + " end";
            thread.CurrentNode = Parent;
            return result;
            */
        }
    }
}
