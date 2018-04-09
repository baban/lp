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
        public AstNode VarNode;
        public AstNode Value { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            VarNode = AddChild("Right", nodes[0]);
            Value = AddChild("Value", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var value = (Object.LpObject)Value.Evaluate(thread);
            var reference = (object[])VarNode.Evaluate(thread);

            var Varname = (string)reference[0];
            var dic = (IDictionary<string, object>)reference[1];
            dic[Varname] = value;

            thread.CurrentNode = Parent;

            return value;
        }
    }
}
