using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class VariableReference : LpBase
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

        IDictionary<string,object> searchContext(Scope scope)
        {
            var dic = scope.AsDictionary();
            if ( !dic.ContainsKey("variables") ) {
                dic["variables"] = new Dictionary<string, object>();
            }
            var vdic = (Dictionary<string, object>)dic["variables"];

            if (vdic.ContainsKey(Varname))
            {
                return vdic;
            } else if(scope.Parent != null)
            {
                return searchContext(scope.Parent);
            } else {
                return vdic;
            }
        }
    }
}
