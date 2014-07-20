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
        }

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            return Object.LpArray.initialize( ChildNodes.Select( (node) => node.DoEvaluate() ).ToArray() );
        }

        public override string toSource( bool expand=false )
        {
            return string.Format("[{0}]", string.Join(", ", ChildNodes.Select((node) => node.toSource(expand))));
        }

        public static LpAstArray toNode(List<object[]> nodes)
        {
            return new Ast.LpAstArray(nodes.Select((o) => LpParser.toNode(o)).ToList());
        }
    }
}
