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
        bool loose = false;

        public LpAstLambda(List<LpAstNode> nodes, string[] args, bool argLoose = false)
        {
            this.args = args;
            this.loose = argLoose;
            ChildNodes = nodes;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            return Object.LpLambda.initialize(ChildNodes, args, loose);
        }
    }
}
