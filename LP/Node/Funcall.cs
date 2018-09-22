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

            var name = functionName.Token.Text;

            var function = searchMetod(scope, name);
            var result = (Object.LpObject)EvaluateInStmts(function, thread);

            thread.CurrentNode = Parent;

            return result;
        }

        Object.LpObject searchMetod(Scope scope, string name) {
            var ret = searchContext(scope, name);

            if (ret != null)
                return ret;

            var klass = currentClass(scope);

            Object.LpMethod m = klass.searchMethod(name, klass);
            if (m != null)
                return Object.LpMethod.initialize(m.method);

            throw new Error.LpNoMethodError();
        }

        Object.LpObject currentClass(Scope scope)
        {
            return Object.LpKernel.initialize();
        }

        Object.LpObject searchContext(Scope scope, string name)
        {
            var dic = Util.Scope.findDictionary(scope, "methods");

            if (dic.ContainsKey(name))
                return (Object.LpObject)dic[name];

            if (scope.Parent == null)
                return null;

            return searchContext(scope.Parent, name);
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
