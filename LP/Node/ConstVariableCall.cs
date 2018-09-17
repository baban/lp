using System.Linq;
using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class ConstVariableCall : LpBase
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
            if (!dic.ContainsKey("const_variables"))
            {
                dic["const_variables"] = new Dictionary<string, object>();
            }
            var cdic = (Dictionary<string, object>)dic["const_variables"];

            Object.LpObject value = null;
            if (cdic.ContainsKey(Varname))
            {
                value = (Object.LpObject)cdic[Varname];
            }
            else if (isBinaryClassName(Varname))
            {
                value = binaryClassCall(Varname);
            } else {
                throw new Error.NameError();
            }

            thread.CurrentNode = Parent;

            return value;
        }

        bool isBinaryClassName(string name)
        {
            return name == "Console";
        }

        private Object.LpObject binaryClassCall(string className)
        {
            var klass = Object.LpClass.initialize(className, true);
            return klass;
        }
    }
}
