using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Funcall : LpBase
    {
        public ParseTreeNode functionName { get; private set; }
        public AstNode Args { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            functionName = nodes[0];
            Args = AddChild("Args", nodes[1]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;

            var scope = thread.CurrentScope;

            var dic = Util.Scope.findDictionary(scope, "methods");

            var name = functionName.Token.Text;

            if (!dic.ContainsKey(name)) {
                throw new Error.LpNoMethodError();
            }

            var function = (Object.LpObject)dic[name];
            var result = (Object.LpObject)EvaluateInStmts(function, thread);

            thread.CurrentNode = Parent;

            return result;
        }

        object EvaluateInStmts(Object.LpObject function, ScriptThread thread)
        {
            var newScopeInfo = new ScopeInfo(thread.CurrentNode, false);

            var args = (Object.LpObject[])Args.Evaluate(thread);
            thread.PushClosureScope(newScopeInfo, thread.CurrentScope, args);

            var scope = thread.CurrentScope;

            var dic = scope.AsDictionary();
            dic["methods"] = new Dictionary<string, object>();
            dic["variables"] = new Dictionary<string, object>();
            var methods = (Dictionary<string, object>)dic["methods"];
            var variables = (Dictionary<string, object>)dic["variables"];
            /*
            var parameters = scope.Parameters;

            for(int i=0; i < args.Length ; i++)
            {
                var v = args[i];
                variables[i.ToString()] = v;
            }
            */
            var result = function.statements.Evaluate(thread);

            thread.PopScope();

            return result;
        }
    }
}
