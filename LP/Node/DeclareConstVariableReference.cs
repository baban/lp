using System.Linq;
using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;


namespace LP.Node
{
    public class DeclareConstVariableReference : LpBase
    {
        string Varname;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            var nodes = treeNode.GetMappedChildNodes();

            Varname = nodes.Last().Token.Text;
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();
            if (!dic.ContainsKey("const_variables"))
            {
                dic["const_variables"] = new Dictionary<string, object>();
            }
            var vdic = (Dictionary<string, object>)dic["const_variables"];

            thread.CurrentNode = Parent;

            return new object[] { Varname, vdic };
        }
    }
}
