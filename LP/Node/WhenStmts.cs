using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class WhenStmts : LpBase
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("WhenStmt", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var result = ChildNodes.Select((node)=> { return (AstNode[])node.Evaluate(thread);  }).ToArray();

            thread.CurrentNode = Parent;

            return result;
        }
    }
}

