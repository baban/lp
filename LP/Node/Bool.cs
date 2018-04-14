using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Bool : LpBase
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var node = treeNode.GetMappedChildNodes().First();
            Node = AddChild("Boolean", node);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var boolValue = Node.Term.ToString() == "true" ? true : false;
            Object.LpObject result = Object.LpBool.initialize(boolValue);
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
