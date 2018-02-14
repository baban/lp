using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace IronyParser.Node
{
    public class AssocVal : AstNode
    {
        public AstNode Key { get; private set; }
        public AstNode Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Key = AddChild("Key", nodes[0]);
            Value = AddChild("Value", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            string result = Key.Evaluate(thread).ToString() + " => " + Value.Evaluate(thread).ToString();

            thread.CurrentNode = Parent;
            return result;
        }
    }
}
