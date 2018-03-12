using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DefineFunction : AstNode
    {
        public ParseTreeNode functionName { get; private set; }
        public AstNode functionBody { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            functionName = nodes[1];
            functionBody = AddChild("Body", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string name = functionName.Token.Text;
            var scope = thread.CurrentScope;
            var slot = scope.AddSlot(name);
            var function = Object.LpBlock.initialize(functionBody);
            scope.SetValue(slot.Index, function);
            thread.CurrentNode = Parent;

            return function;
        }
    }
}

