using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class SpecifierReference : LpBase
    {
        AstNode Expr;
        string Varname;
        AstNode Node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();
            Expr = AddChild("Expr", nodes[0]);
            Varname = nodes[1].Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var obj = (Object.LpObject)Expr.Evaluate(thread);

            thread.CurrentNode = Parent;

            return new object[] { Varname, obj.variables };
        }
    }
}
