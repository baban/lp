using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ElsIfStmts : LpBase
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("IfStmt", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            Object.LpObject result = null;
            thread.CurrentNode = this;

            foreach( var node in ChildNodes)
            {
                result = (Object.LpObject)node.Evaluate(thread);
                if (result != null) return result;
            }

            thread.CurrentNode = Parent;

            return result;
        }
    }
}

