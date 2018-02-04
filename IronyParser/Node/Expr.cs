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
    public class Expr : AstNode
    {
        public AstNode Left { get; private set; }
        public AstNode Right { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            Left = AddChild("Left", nodes[0]);
            Right = AddChild("Right", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            string result = Left.Evaluate(thread).ToString() + "+" + Right.Evaluate(thread).ToString();

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
