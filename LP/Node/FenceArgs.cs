using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class FenceArgs : LpBase
    {
        AstNode VarNames;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            //System.Console.WriteLine(nodes.Count());
            //nodes.ForEach((node) => System.Console.WriteLine(node));
            //nodes.ForEach((node) => AddChild("Stmt", node));
            VarNames = AddChild("VarNames", nodes[1]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            //Object.LpObject[] result = ChildNodes.Select((node) => (Object.LpObject)node.Evaluate(thread)).ToArray();
            thread.CurrentNode = Parent;
            //return result;
            return null;
        }
    }
}

