using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Assignment : AstNode
    {
        public ParseTreeNode VarNode;
        public AstNode Right { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            VarNode = nodes[0];
            Right = AddChild("Right", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string Varname = VarNode.ChildNodes.First().Token.Text;
            var right = (Object.LpObject)Right.Evaluate(thread);
            var scope = thread.CurrentScope;
            var slot = scope.AddSlot(Varname);
            scope.SetValue(slot.Index, right);
            var value = (Object.LpObject)thread.CurrentScope.GetValue(slot.Index);
            thread.CurrentNode = Parent;
            return value;
        }
    }
}
