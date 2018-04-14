using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Regex : LpBase
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var node = treeNode.GetMappedChildNodes().First();
            Node = AddChild("Regex", node);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject result = Object.LpString.initialize("");
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
