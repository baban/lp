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

            if (nodes.Count >= 1)
            {
                Stmts = AddChild("Stmts", nodes[1]);
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            Object.LpObject result = null;
            if(Stmts != null) {
                result = (Object.LpObject)Stmts.Evaluate(thread);
            } else
            {
                result = Object.LpNl.initialize();
            }

            thread.CurrentNode = Parent;

            return result;
        }
    }
}

