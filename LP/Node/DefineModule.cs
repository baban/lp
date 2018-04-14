using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DefineModule : LpBase
    {
        public ParseTreeNode className { get; private set; }
        public AstNode Body { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            className = nodes[2];
            Body = AddChild("Body", nodes[4]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var klass = Object.LpNl.initialize();
            /*
            var klass = Object.LpClass.initialize(className.Token.Text, Body);
            var scope = thread.CurrentScope;
            var newScopeInfo = new ScopeInfo(thread.CurrentNode, false);
            thread.PushClosureScope(newScopeInfo, thread.CurrentScope, new object[] { });
            Body.Evaluate(thread);
            thread.PopScope();

            */
            thread.CurrentNode = Parent;

            return klass;
        }
    }
}

