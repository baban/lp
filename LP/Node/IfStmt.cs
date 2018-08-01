using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class IfStmt : LpBase
    {
        AstNode Expr;
        AstNode Stmts;
        AstNode ElsIfStmts;
        AstNode ElseStmt;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => {
                if(node.AstNode != null)
                {
                    var t = node.AstNode.GetType().ToString();
                    switch (t)
                    {
                        case "LP.Node.Stmts":
                            Stmts = AddChild("Stmts", node);
                            break;
                        case "LP.Node.ElsIfStmts":
                            ElsIfStmts = AddChild("ElsIfStmts", node);
                            break;
                        case "LP.Node.ElseStmt":
                            ElseStmt = AddChild("ElseStmts", node);
                            break;
                    }
                }
            });
            Expr = AddChild("IfExpr", nodes[1]);
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
            else if (ElsIfStmts != null)
            {
                result = (Object.LpObject)ElsIfStmts.Evaluate(thread);
            }

            if (result != null && ElseStmt != null)
            {
                result = (Object.LpObject)ElseStmt.Evaluate(thread);
            } else
            {
                result = Object.LpNl.initialize();
            }

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
