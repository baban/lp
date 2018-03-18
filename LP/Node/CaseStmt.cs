using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class CaseStmt : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return null;
            /*
            thread.CurrentNode = this;
            string result = "if(" + Expr.Evaluate(thread).ToString() + ")" + Stmts.Evaluate(thread).ToString() + " end";
            thread.CurrentNode = Parent;
            return result;
            */
        }
    }
}
