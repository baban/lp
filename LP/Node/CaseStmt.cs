using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class CaseStmt : AstNode
    {
        AstNode Expr = null;
        AstNode WhenStmts = null;
        AstNode ElseStmt = null;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("CaseExpr", nodes[1]);
            nodes.ForEach((node) => {
                if (node.AstNode != null)
                {
                    var t = node.AstNode.GetType().ToString();
                    switch (t)
                    {
                        case "LP.Node.WhenStmts":
                            WhenStmts = AddChild("WhenStmts", node);
                            break;
                        case "LP.Node.ElseStmt":
                            ElseStmt = AddChild("ElseStmts", node);
                            break;
                    }
                }
            });
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var expr = (Object.LpObject)Expr.Evaluate(thread);
            var whenStmts = (AstNode[][])WhenStmts.Evaluate(thread);
            var result = cond(thread, expr, whenStmts);
            if(result == null)
            {
                 result = (ElseStmt != null) ? (Object.LpObject)ElseStmt.Evaluate(thread) : Object.LpNl.initialize();
            }
            thread.CurrentNode = Parent;

            return result;
        }

        private Object.LpObject cond(ScriptThread thread, Object.LpObject baseValue, AstNode[][] childNodes)
        {
            Object.LpObject result = null;
            foreach (var node in childNodes)
            {
                var pair = node;
                var expr = pair[0];
                var stmts = pair[1];
                var check = (Object.LpObject)(expr.Evaluate(thread));
                if ( (bool)check.funcall("==", new Object.LpObject[]{ baseValue }).boolValue )
                {
                    result = (Object.LpObject)(stmts.Evaluate(thread));
                    break;
                }
            }
            return result;
        }
    }
}
