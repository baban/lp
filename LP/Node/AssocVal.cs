﻿using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class AssocVal : LpBase
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
            Object.LpObject[] result = new Object.LpObject[]{ (Object.LpObject)Key.Evaluate(thread), (Object.LpObject)Value.Evaluate(thread) };
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
