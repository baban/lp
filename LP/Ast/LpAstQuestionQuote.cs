using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstQuestionQuote : LpAstNode
    {
        public LpAstQuestionQuote(LpAstNode node)
        {
            ChildNodes = new List<LpAstNode>() { node };
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate()
        {
            return this.DoEvaluate();
        }

        public override string toSource()
        {
            return string.Format("?{0}", string.Join("", ChildNodes.Select((node) => node.toSource())));
        }

        public override LpAstNode DoExpand()
        {
            var expr = ChildNodes.First().DoEvaluate().funcall("to_s", null, null).stringValue;
            return LpParser.createNode(expr);
        }

        public static LpAstQuestionQuote toNode(object[] node)
        {
            return new Ast.LpAstQuestionQuote(LpParser.toNode((object[])node));
        }
    }
}
