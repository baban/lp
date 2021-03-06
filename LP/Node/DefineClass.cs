﻿using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DefineClass : LpBase
    {
        public ParseTreeNode className { get; private set; }
        public AstNode Body { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            System.Console.WriteLine(nodes[2].Token.Text);
            className = nodes[2];
            Body = AddChild("Body", nodes[3]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var scope = thread.CurrentScope;
            var newScopeInfo = new ScopeInfo(thread.CurrentNode, false);
            var klass = Object.LpClass.initialize(className.Token.Text, Body, false, scope);
            thread.PushClosureScope(newScopeInfo, thread.CurrentScope, new object[] { });
            Body.Evaluate(thread);
            thread.PopScope();

            thread.CurrentNode = Parent;

            return klass;
        }
    }
}

