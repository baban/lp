using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Primary : AstNode
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var node = treeNode.GetMappedChildNodes().First();
            Node = AddChild("Primary", node);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            string result = "";
            if(Node is Symbol || Node is Array)
            {
                result = Node.Evaluate(thread).ToString();
            } else
            {
                result = Node.ToString();
            }
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
