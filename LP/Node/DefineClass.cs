﻿using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DefineClass : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var scope = thread.CurrentScope;
            var Varname = "aaa";
            var right = (Object.LpObject)Object.LpNl.initialize();
            var slot = scope.AddSlot(Varname);
            scope.SetValue(slot.Index, right);

            thread.CurrentNode = Parent;
            return Object.LpNl.initialize();
        }
    }
}

