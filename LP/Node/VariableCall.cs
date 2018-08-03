using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class VariableCall : LpBase
    {
        public AstNode Node { get; private set; }
        public ParseTreeNode node;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            node = nodes.Last();
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string Varname = node.Token.Text;

            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();

            Object.LpObject value = null;
            if (dic.ContainsKey(Varname))
            {
                value = (Object.LpObject)dic[Varname];
            } else if(Varname == "Console")
            {
                System.Console.WriteLine("Console");
                value = classCall(Varname);
            }

            thread.CurrentNode = Parent;

            return value;
        }

        private Object.LpObject classCall(string className)
        {
            var klass = Object.LpClass.initialize(className);
            return klass;
        }
    }
}
