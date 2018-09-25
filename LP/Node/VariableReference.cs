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
            var dic = Util.Scope.searchContext(scope, "variables", Varname);

            thread.CurrentNode = Parent;

            return new object[] { Varname,  dic };
        }
    }
}
