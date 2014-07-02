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

        public override Object.LpObject DoEvaluate()
        {
            return Object.LpArray.initialize( ChildNodes.Select( (node) => node.DoEvaluate() ).ToArray() );
        }

        public virtual string toSource()
        {
            return string.Format("[{0}]", string.Join(", ", ChildNodes.Select((node) => node.toSource())));
        }
    }
}
