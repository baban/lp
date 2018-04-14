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
            try
            {
                var nodes = treeNode.GetMappedChildNodes();
                nodes.ForEach((node) => AddChild("Stmt", node));
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return;
            }
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
                var traces = thread.GetStackTrace();
                var location = thread.CurrentNode.Location;
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(location);
                return null;
            }
        }
    }
}
