using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class WhenStmt : LpBase
    {
        AstNode Expr = null;
        AstNode Stmts = null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("CaseExpr", nodes[1]);
            Stmts = AddChild("Stmts", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            return new AstNode[] { Expr, Stmts };
        }
    }
}

