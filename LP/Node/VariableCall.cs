using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class VariableCall : AstNode
    {
        public AstNode Node { get; private set; }
        public ParseTreeNode node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            node = nodes.Last();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            thread.CurrentNode = Parent;
            return null;
        }
    }
}
