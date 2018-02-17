using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstQuote : LpAstNode
    {
        public LpAstQuote( LpAstNode node )
        {
            ChildNodes = new List<LpAstNode>() { node };
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate()
        {
            return LP.Object.LpQuote.initialize( ChildNodes );
        }

        public override LpAstNode DoExpand()
        {
            return this;
        }

        public override string toSource()
        {
            return string.Format("'{0}", string.Join("", ChildNodes.Select((node) => node.toSource())));
        }

        public static LpAstQuote toNode(object[] node)
        {
            return new Ast.LpAstQuote(LpParser.toNode((object[])node));
        }
    }
}
