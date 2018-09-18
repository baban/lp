using System.Linq;
using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DeclareVariableReference : LpBase
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
            var dic = Util.Scope.findDictionary(scope, "variables");

            thread.CurrentNode = Parent;

            return new object[] { Varname, dic };
        }
    }
}
