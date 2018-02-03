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
    public class LpNode : AstNode
    {
        ParseTreeNodeList nodes;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            Console.WriteLine("treeNode");
            Console.WriteLine(treeNode);
            Console.WriteLine(Location);
            nodes = treeNode.GetMappedChildNodes();
        }


        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            thread.CurrentNode = Parent;

            return nodes.First().ToString();
        }
    }
}
