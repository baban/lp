using System;
using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Op : AstNode
    {
        public string Node { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Node = nodes[0].ToString();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string result = Node.ToString();
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
