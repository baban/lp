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

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            return LP.Object.LpQuote.initialize( ChildNodes );
        }

        public new LpAstNode DoExpand()
        {
            ChildNodes = ChildNodes.Select((node) => node.Expand()).ToList();

            return this;
        }

        public override string toSource(bool expand = false)
        {
            return string.Format("'{0}", string.Join("", ChildNodes.Select((node) => node.toSource(expand))));
        }

        public static LpAstQuote toNode(object[] node)
        {
            return new Ast.LpAstQuote(LpParser.toNode((object[])node));
        }
    }
}
