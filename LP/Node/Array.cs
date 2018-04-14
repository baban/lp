using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Array : LpBase
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count > 2)
            {
                AddChild("Node", nodes[1]);
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            Object.LpObject[] items = null;
            thread.CurrentNode = this;
            if (ChildNodes.Count() > 0)
            {
                items = (Object.LpObject[])ChildNodes.First().Evaluate(thread);
            }
            thread.CurrentNode = Parent;
            return (items==null) ? Object.LpArray.initialize() : Object.LpArray.initialize(items);
        }
    }
}

