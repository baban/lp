using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Funcall : AstNode
    {
        public ParseTreeNode functionName { get; private set; }
        public AstNode Args { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            functionName = nodes[0];
            Args = AddChild("Args", nodes[1]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var scope = thread.CurrentScope;
            var slot = scope.Info.GetSlot(functionName.Token.Text);
            var function = (Object.LpObject)scope.GetValue(slot.Index);

            thread.PushClosureScope(new ScopeInfo(thread.CurrentNode, false), thread.CurrentScope, new object[]{ });
            Object.LpObject ret = Object.LpNl.initialize();
            var result = function.statements.Evaluate(thread);
            thread.PopScope();

            thread.CurrentNode = Parent;
            return result;
        }
    }
}
