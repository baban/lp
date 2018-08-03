using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ElsIfStmt : LpBase
    {
        AstNode Expr = null;
        AstNode Stmts = null;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("Expr", nodes[1]);
            Stmts = AddChild("Stmts", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var check = (Object.LpObject)Expr.Evaluate(thread);

            Object.LpObject result = null;
            if ((bool)check.boolValue) {
                result = (Object.LpObject)Stmts.Evaluate(thread);
            }

            thread.CurrentNode = Parent;

            return result;
        }
    }
}

