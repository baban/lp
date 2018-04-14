using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ArrayAtExpr : LpBase
    {
        public AstNode Expr { get; private set; }
        public ParseTreeNode functionName { get; private set; }
        public AstNode Arg { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("Expr", nodes[0]);
            Arg = AddChild("Arg", nodes[1]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var val = (Object.LpObject)Expr.Evaluate(thread);
            var arg = (Object.LpObject)Arg.Evaluate(thread);
            var args = new Object.LpObject[] { arg };
            // var result = val.funcall("[]", args);

            thread.CurrentNode = Parent;

            //return result;
            return Object.LpNl.initialize();
        }
    }
}
