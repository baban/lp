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
    public class Symbol : AstNode
    {
        public AstNode Node { get; private set; }
        public ParseTreeNode node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            node = treeNode.GetMappedChildNodes().First();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            thread.CurrentNode = Parent;
            return "Symbvol";
        }
    }
}
