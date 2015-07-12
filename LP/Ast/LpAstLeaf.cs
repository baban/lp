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
            init();
        }

        public LpAstLeaf(string n, string type)
        {
            this.leaf = n;
            this.type = type;
            init();
        }

        public void init() {
            this.Evaluate = DoEvaluate;
            this.Source = toSource;
        }

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            switch (type) {
                case "NL":
                    return Object.LpNl.initialize();
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
                    return Object.LpQuasiQuote.initialize(LpParser.toNode(LpParser.PROGRAM.Parse(leaf)).toSource(expand));
                case "QUESTION_QUOTE":
                    return LpParser.toNode(LpParser.PROGRAM.Parse(leaf)).DoEvaluate().funcall("to_s", null, null);
                case "VARIABLE_CALL":
                    return Util.LpIndexer.varcall(leaf);
                case "GLOBAL_VARIABLE_CALL":
                    return Util.LpGlobalIndexer.get(leaf);
                case "INSTANCE_VARIABLE_CALL":
                    return Util.LpGlobalIndexer.get(leaf);
                default:
                    return null;
            }
        }
        
        public override string toSource( bool expand=false )
        {
            switch (type) {
                case "QUOTE":
                    return string.Format("'{0}", leaf);
                case "QUASI_QUOTE":
                    return string.Format("`{0}", leaf);
                case "QUESTION_QUOTE":
                    return expand ?
                        LpParser.toNode(LpParser.PROGRAM.Parse(leaf)).DoEvaluate().funcall("to_s",null,null).stringValue :
                        string.Format("?{0}", leaf);
                default:
                    return leaf;
            }
        }

        public static LpAstLeaf toNode(string leaf, string type)
        {
            switch (type) {
                case "NL":
                case "NUMERIC":
                case "STRING":
                case "BOOL":
                case "SYMBOL":
                case "VARIABLE_CALL":
                case "GLOBAL_VARIABLE_CALL":
                case "INSTANCE_VARIABLE_CALL":
                case "QUOTE":
                case "QUASI_QUOTE":
                case "QUESTION_QUOTE":
                    return new Ast.LpAstLeaf(leaf, type);
                default:
                    return null;
            }
        }
    }
}
