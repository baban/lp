using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class IfStmt : LpBase
    {
        public AstNode Expr { get; private set; }
        public AstNode Stmts { get; private set; }
        public AstNode ElseStmts { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("IfExpr", nodes[1]);
            Stmts = AddChild("Stmts", nodes[2]);
            if(nodes.Count > 4)
            {
                ElseStmts = AddChild("ElseStmts", nodes[4]);
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var check = (Object.LpObject)Expr.Evaluate(thread);

            Object.LpObject result = null;
            if ((bool)check.boolValue)
            {
                result = (Object.LpObject)Stmts.Evaluate(thread);
            }
            else
            {
                if (ElseStmts != null)
                {
                    result = (Object.LpObject)ElseStmts.Evaluate(thread);
                }
                else
                {
                    result = null;
                }
            }
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
