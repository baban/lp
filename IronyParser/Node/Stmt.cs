using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class Stmt : AstNode
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Node = AddChild("Left", nodes[0]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string result = "";
            result = Node.Evaluate(thread).ToString();
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
