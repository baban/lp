using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstHash : LpAstNode
    {
        List<LpAstNode[]> pairs = null;

        public LpAstHash(List<LpAstNode[]> pairs)
        {
            this.pairs = pairs;

            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            Object.LpObject ret = Object.LpHash.initialize( this.pairs );
            return ret;
        }

        public override LpAstNode DoExpand()
        {
            pairs = pairs.Select((pair) => {
                pair[0] = pair[0].DoExpand();
                pair[1] = pair[1].DoExpand();
                return pair;
            }  ).ToList();
            return this;
        }

        public override string toSource(bool expand = false)
        {
            return "{ " +
                string.Join(", ", pairs.Select((pair) => {
                    return string.Join(" : ", pair.Select((node) => node.toSource(expand)));
                }).ToArray() ) +
                   " }";
        }

        public static LpAstHash toNode(object[] nodes)
        {
            var pairs = nodes.Select((pair) =>
            {
                var pr = (object[])pair;
                return new Ast.LpAstNode[] { LpParser.toNode((object[])pr[0]), LpParser.toNode((object[])pr[1]) };
            }).ToList();
            return new Ast.LpAstHash(pairs);
        }
    }
}
