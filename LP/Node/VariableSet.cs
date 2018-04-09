using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class VariableSet : AstNode
    {
        AstNode Node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            Node = AddChild("Node", nodes[0]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var state = Node.Evaluate(thread);
            //var scope = thread.CurrentScope;
            //var dic = scope.AsDictionary();

            thread.CurrentNode = Parent;
            //return new object[] { "a", dic };
            return state;
        }
    }
}
