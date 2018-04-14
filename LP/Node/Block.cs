using System.Collections.Generic;
using System.Linq;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace LP.Node
{
    public class Block : LpBase
    {
        AstNode ArgVarNames = null;
        AstNode Body = null;

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            ArgVarNames = AddChild("ArgVarNames", nodes[1]);
            Body = AddChild("Body", nodes[2]);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            Object.LpObject result = null;
            if (ChildNodes.Count() > 0)
            {
                result = Object.LpBlock.initialize(Body);
                result.arguments = null;
            }
            else
            {
                result = Object.LpBlock.initialize();
            }
            thread.CurrentNode = Parent;
            return result;
        }
    }
}
