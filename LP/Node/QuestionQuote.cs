using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class QuestionQuote : AstNode
    {
        public AstNode Node { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var node = treeNode.GetMappedChildNodes().Last();
            Node = AddChild("QuotedVariable", node);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var quote = (Object.LpObject)Node.Evaluate(thread);
            var result = (Object.LpObject)quote.statements.Evaluate(thread);
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
