using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class WhenStmt : LpBase
    {
        public AstNode Expr { get; private set; }
        public AstNode Stmts { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("CaseExpr", nodes[1]);
            Stmts = AddChild("Stmts", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject result = Object.LpNl.initialize();

            thread.CurrentNode = Parent;

            return result;
        }
    }
}

