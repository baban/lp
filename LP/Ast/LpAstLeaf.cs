using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Ast
{
    class LpAstLeaf : LpAstNode
    {
        string leaf = null;

        public LpAstLeaf(string n) {
            leaf = n;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            Console.WriteLine("AstNode");
            return Object.LpNumeric.initialize(Double.Parse(leaf));
        }

    }
}
