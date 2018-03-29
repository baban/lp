using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class MethodCall : AstNode
    {
        public AstNode Expr { get; private set; }
        public ParseTreeNode functionName { get; private set; }
        public AstNode Args { get; private set; }
        public AstNode BlockArg { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("Expr", nodes[0]);
            functionName = nodes[1];
            Args = AddChild("Args", nodes[2]);
            BlockArg = AddChild("BlockArg", nodes[3]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var val = (Object.LpObject)Expr.Evaluate(thread);
            var args = (Object.LpObject[])Args.Evaluate(thread);
            var result = val.funcall(functionName.Token.Text, args);

            thread.CurrentNode = Parent;

            return result;
        }
    }
}
