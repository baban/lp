using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Stmt : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();

            nodes.ForEach((node) => AddChild("Node", node) );
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            string result = ChildNodes.Select((node) => node.Evaluate(thread) + ";").Aggregate((a, b) => a + b).ToString();

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
