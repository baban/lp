using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstQuasiQuote : LpAstNode
    {
        public LpAstQuasiQuote( LpAstNode node )
        {
            ChildNodes = new List<LpAstNode>() { node };
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate()
        {
            return LP.Object.LpQuasiQuote.initialize( ChildNodes );
        }

        public override LpAstNode DoExpand()
        {
            return this;
        }

        public override string toSource()
        {
            return string.Format("`{0}", string.Join("", ChildNodes.Select((node) => node.toSource())));
        }

        public static LpAstQuasiQuote toNode(object[] node)
        {
            return new Ast.LpAstQuasiQuote(LpParser.toNode((object[])node));
        }
    }
}
