using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Args : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("Arg", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject[] result = ChildNodes.Select((node) => (Object.LpObject)node.Evaluate(thread)).ToArray();
            thread.CurrentNode = Parent;

            return result;
        }
    }
}

