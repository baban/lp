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

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            return Object.LpLambda.initialize(ChildNodes, args, loose);
        }

        public override string toSource( bool expand=false )
        {
            return string.Format("->{0} do {1} end",
                toSourceArgs(),
                string.Join("; ", ChildNodes.Select((node) => node.toSource(expand))));
        }

        private string toSourceArgs()
        {
            return string.Format("({0})", string.Join(",", this.args.ToArray()));
        }

        public static LpAstLambda toNode(object[] nodes)
        {
            var blk = nodes;
            return new Ast.LpAstLambda(
                LpParser.toNode((object[])blk[2]).ChildNodes,
                (string[])blk[0],
                (bool)blk[1]);
        }
    }
}
