using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ArgVarnames : LpBase
    {
        ParseTreeNodeList nodes;
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            nodes = treeNode.GetMappedChildNodes();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var scope = thread.CurrentScope;
            nodes.ForEach((node)=> {
                var slot = scope.AddSlot(node.Token.Text);
                //scope.SetParameter(slot.Index, null);
            });
            thread.CurrentNode = Parent;

            return null;
        }
    }
}

