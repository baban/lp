using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstArray : LpAstNode
    {
        public LpAstArray(List<LpAstNode> nodes)
        {
            ChildNodes = nodes;
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate()
        {
            return Object.LpArray.initialize(ChildNodes.Select((node) => node.DoEvaluate()).ToArray());
        }

        public override LpAstNode DoExpand()
        {
            ChildNodes = ChildNodes.Select((node) =>
            {
                return node.DoExpand();
            }).ToList();

            return this;
        }

        public override string toSource()
        {
            return string.Format("[{0}]", string.Join(", ", ChildNodes.Select((node) => node.toSource())));
        }

        public static LpAstArray toNode(List<object[]> nodes)
        {
            return new Ast.LpAstArray(nodes.Select((o) => LpParser.toNode(o)).ToList());
        }
    }
}
