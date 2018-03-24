using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class IfStmt : AstNode
    {
        public AstNode Expr { get; private set; }
        public ParseTreeNode Op;
        public AstNode Stmts { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("IfExpr", nodes[1]);
            Stmts = AddChild("Stmts", nodes[3]);
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
