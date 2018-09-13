using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class SpecifierCall : LpBase
    {
        public AstNode Expr { get; private set; }
        public ParseTreeNode constantName { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("Expr", nodes[0]);
            constantName = nodes[1];
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var val = (Object.LpObject)Expr.Evaluate(thread);
            var result = val.getVariable(constantName.Token.Text);

            thread.CurrentNode = Parent;

            return result;
        }

    }
}
