using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstLambda : LpAstNode
    {
        string[] args;

        public LpAstLambda(List<LpAstNode> nodes, string[] args )
        {
            this.args = args;
            ChildNodes = nodes;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            return Object.LpLambda.initialize( ChildNodes, args );
        }
    }
}
