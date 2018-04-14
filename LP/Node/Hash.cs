using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Hash : LpBase
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            if (nodes.Count() > 0)
            {
                AddChild("Node", nodes.First());
            }
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject result;
            if (ChildNodes.Count() > 0)
            {
                var assoc = ChildNodes.First();
                var pairs = (Dictionary<Object.LpObject, Object.LpObject>)assoc.Evaluate(thread);
                result = Object.LpHash.initialize(pairs);
            } else
            {
                result = Object.LpHash.initialize();
            }
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
