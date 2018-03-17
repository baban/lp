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
            System.Console.WriteLine("QuasiQuote");
            Node = AddChild("QuotedVariable", node);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            System.Console.WriteLine("Node");
            System.Console.WriteLine(Node);
            var var = (Object.LpObject)Node.Evaluate(thread);
            System.Console.WriteLine("aaaaaa");
            System.Console.WriteLine( var.class_name);
            System.Console.WriteLine(var.statements);
            var result = (Object.LpObject)var.statements.Evaluate(thread);
            thread.CurrentNode = Parent;

            return result;
        }
    }
}
