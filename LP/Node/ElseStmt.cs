using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ElseStmt : LpBase
    {
        AstNode Stmts = null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();
            Stmts = AddChild("Stmts", nodes[1]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            Object.LpObject result = (Object.LpObject)Stmts.Evaluate(thread);

            thread.CurrentNode = Parent;

            return result;
        }
    }
}

