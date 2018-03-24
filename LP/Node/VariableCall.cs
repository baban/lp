using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class VariableCall : AstNode
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

            System.Console.WriteLine(Varname);

            var scope = thread.CurrentScope;
            var slot = scope.Info.GetSlot(Varname);
            var value = (Object.LpObject)thread.CurrentScope.GetValue(slot.Index);
            thread.CurrentNode = Parent;
            return value;
        }
    }
}
