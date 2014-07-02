using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstHash : LpAstNode
    {
        List<LpAstNode[]> pairs = null;

        public LpAstHash(List<LpAstNode[]> pairs)
        {
            this.pairs = pairs;

            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            Object.LpObject ret = Object.LpHash.initialize( this.pairs );
            return ret;
        }

        public override string toSource()
        {
            return "{ " +
                string.Join(", ", pairs.Select((pair) => {
                    return string.Join( " : ", pair.Select((node) => node.toSource()) );
                }).ToArray() ) +
                   " }";
        }
    }
}
