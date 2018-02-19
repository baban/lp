using System;
using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class LeftUnary : AstNode
    {
        public AstNode Node { get; private set; }
        public ParseTreeNode Op;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Node = AddChild("Node", nodes[0]);
            Op = nodes[1];
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            string result = Node.Evaluate(thread).ToString() + Op.ToString();

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
