﻿using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ConstVariableCall : LpBase
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
            string Varname = node.Token.Text;

            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();

            Object.LpObject value = null;
            if (dic.ContainsKey(Varname))
            {
                value = (Object.LpObject)dic[Varname];
            }
            else if (isBinaryClassName(Varname))
            {
                value = binaryClassCall(Varname);
            }

            thread.CurrentNode = Parent;

            return value;
        }

        bool isBinaryClassName(string name)
        {
            return name == "Console";
        }

        private Object.LpObject binaryClassCall(string className)
        {
            var klass = Object.LpClass.initialize(className, true);
            return klass;
        }
    }
}