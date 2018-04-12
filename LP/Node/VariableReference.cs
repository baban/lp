using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class VariableReference : AstNode
    {
        string Varname;
        AstNode Node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();

            Node = AddChild("Node", nodes[0]);
            Varname = nodes[0].Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;


            var scope = thread.CurrentScope;
            var dic = searchContext(scope); 

            thread.CurrentNode = Parent;

            return new object[] { Varname,  dic };
        }

        System.Collections.Generic.IDictionary<string,object> searchContext(Scope scope)
        {
            var dic = scope.AsDictionary();

            if (dic.ContainsKey(Varname))
            {
                return dic;
            } else if(scope.Parent != null)
            {
                return searchContext(scope.Parent);
            }
            else
            {
                return dic;
            }
        }
    }
}
