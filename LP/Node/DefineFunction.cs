using System.Collections.Generic;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DefineFunction : LpBase
    {
        public ParseTreeNode functionName { get; private set; }
        public AstNode CallArgs { get; private set; }
        public AstNode Body { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            functionName = nodes[2];
            CallArgs = AddChild("CallArgs", nodes[3]);
            Body = AddChild("Body", nodes[4]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            string name = functionName.Token.Text;
            var scope = thread.CurrentScope;
            var dic = scope.AsDictionary();

            if (!dic.ContainsKey("methods"))
            {
                dic["methods"] = new Dictionary<string, object>();
            }
            var fdic = (Dictionary<string, object>)dic["methods"];

            var function = Object.LpBlock.initialize(Body, CallArgs);
            fdic[name] = function;

            thread.CurrentNode = Parent;

            return function;
        }
    }
}

