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
        string type = null;

        public LpAstLeaf(string n) {
            leaf = n;
            this.Evaluate = DoEvaluate;
        }

        public LpAstLeaf(string n, string type)
        {
            leaf = n;
            this.type = type;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            switch (type) {
                case "INT":
                    return Object.LpNumeric.initialize(Double.Parse(leaf));
                case "DOUBLE":
                    return Object.LpNumeric.initialize(Double.Parse(leaf));
                case "NUMERIC":
                    return Object.LpNumeric.initialize(Double.Parse(leaf));
                case "BOOL":
                    return Object.LpBool.initialize( Boolean.Parse(leaf) );
                case "STRING":
                    return Object.LpString.initialize( leaf );
                case "SYMBOL":
                    return Object.LpSymbol.initialize( leaf );
                case "QUOTE":
                    return Object.LpQuote.initialize( leaf );
                case "QUASI_QUOTE":
                    return Object.LpQuote.initialize( leaf );
                case "VARIABLE_CALL":
                    return Util.LpIndexer.last().varcall( leaf );
                default:
                    return null;
            }
        }

    }
}
