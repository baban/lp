using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Assoc : AstNode
    {
        public AstNode Pairs { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("Assoc", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var result = ChildNodes.Select((node) => (Object.LpObject[])node.Evaluate(thread)).ToDictionary( pairs => pairs[0], pairs => pairs[1] );
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
