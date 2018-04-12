using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DeclareVariableReference : AstNode
    {
        string Varname;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();

            Varname = nodes.Last().Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();

            thread.CurrentNode = Parent;

            return new object[] { Varname, dic };
        }
    }
}
