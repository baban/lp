using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstBlock : LpAstNode
    {
        string[] args;

        public LpAstBlock(List<LpAstNode> nodes, string[] args, bool argLoose = false )
        {
            this.args = args;
            ChildNodes = nodes;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            return Object.LpBlock.initialize( ChildNodes, args );
        }
    }
}
