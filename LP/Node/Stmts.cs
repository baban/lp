using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Stmts : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("Stmt", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            Object.LpObject result = Object.LpNl.initialize();
            thread.CurrentNode = this;
            ChildNodes.ForEach((node) => result = (Object.LpObject)node.Evaluate(thread));
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
