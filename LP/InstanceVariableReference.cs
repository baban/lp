using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class InstanceVariableReference : AstNode
    {
        string Varname;
        AstNode Node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();

            Node = AddChild("Node", nodes[0]);
            Varname = nodes[0].Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            System.Console.WriteLine("aaaaa");

            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();

            thread.CurrentNode = Parent;

            return new object[] { Varname, dic };
        }
    }
}
