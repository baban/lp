using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstQuote : LpAstNode
    {
        public LpAstQuote( string leaf )
        {
            LpAstNode node = LpParser.createNode(leaf);
            ChildNodes = new List<LpAstNode>() { node };
            this.Evaluate = DoEvaluate;
        }

        public LpAstQuote( LpAstNode node )
        {
            ChildNodes = new List<LpAstNode>() { node };
            this.Evaluate = DoEvaluate;
        }

        public LpAstQuote(List<LpAstNode> nodes)
        {
            ChildNodes = nodes;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            Object.LpObject ret = Object.LpNl.initialize();
            ChildNodes.ForEach((node) => { ret = node.Evaluate(true); });
            return ret;
        }

        public override string toSource(bool expand = false)
        {
            return string.Format("'{0}", string.Join("", ChildNodes.Select((node) => node.toSource(expand))));
        }
    }
}
