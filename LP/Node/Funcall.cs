using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Funcall : AstNode
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
            var slot = scope.Info.GetSlot(functionName.Token.Text);
            var function = (Object.LpObject)scope.GetValue(slot.Index);
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
            var prms = scope.Parameters;
            var names = function.arguments.arguments;
            Queue<Object.LpObject> q = new Queue<Object.LpObject>(new List<Object.LpObject>(args));
            for(int i=0; i < names.Length ; i++)
            {
                var name = names[i];
                var slot = newScopeInfo.AddSlot(name, SlotType.Parameter);
                var v = args[i];
                scope.SetValue(slot.Index, v);
            }
            var result = function.statements.Evaluate(thread);
            thread.PopScope();
            return result;
        }
    }
}
