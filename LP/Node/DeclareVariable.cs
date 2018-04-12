using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DeclareVariable : AstNode
    {
        string Varname;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Varname = nodes[0].Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var ret = Object.LpNl.initialize();
            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();
            dic[Varname] = ret;

            thread.CurrentNode = Parent;

            return ret;
        }
    }
}
