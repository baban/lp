using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class DefineClass : AstNode
    {
        public ParseTreeNode className { get; private set; }
        public AstNode Body { get; private set; }
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            className = nodes[1];
            Body = AddChild("Body", nodes[3]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var klass = Object.LpClass.initialize(className.Token.Text, Body);
            thread.CurrentNode = Parent;
            return klass;
        }
    }
}

