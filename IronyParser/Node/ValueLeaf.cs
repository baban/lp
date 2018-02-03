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
    class ValueLeaf : AstNode
    {
        public AstNode Leaf { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            Console.WriteLine("Init");
            //ParseTreeNodeList nodes = treeNode.GetMappedChildNodes();
            //Leaf = AddChild("Leaf", nodes[0]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            Console.WriteLine("DoEvaluate");
            //string result = Leaf.Evaluate(thread).ToString();

            thread.CurrentNode = Parent;

            return "aaaa";
        }
    }
}
