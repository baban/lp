using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Stmts : LpBase
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            nodes.ForEach((node) => AddChild("Stmt", node));
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            try {
                Object.LpObject result = Object.LpNl.initialize();
                thread.CurrentNode = this;
                ChildNodes.ForEach((node) => result = (Object.LpObject)node.Evaluate(thread));
                thread.CurrentNode = Parent;
                return result;
            }
            catch(System.Exception e)
            {
                System.Console.WriteLine("aaaaaaa");
                var traces = thread.GetStackTrace();
                var location = thread.CurrentNode.Location;
                System.Console.WriteLine(location);
                return null;
            }
        }
    }
}
