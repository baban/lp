using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstBlock : LpAstNode
    {
        string[] args;

        public LpAstBlock(List<LpAstNode> nodes, string[] args, bool argLoose = false )
        {
            this.args = args;
            ChildNodes = nodes;
            init();
        }

        public void init()
        {
            this.Evaluate = DoEvaluate;
            this.Source = toSource;
        }

        public override Object.LpObject DoEvaluate()
        {
            return Object.LpBlock.initialize( ChildNodes, args );
        }

        public override string toSource( bool expand=false )
        {
            return string.Format("do {0}{1} end",
                toSourceArgs(),
                string.Join("; ", ChildNodes.Select((node) => node.toSource(expand))));
        }

        private string toSourceArgs()
        {
            if (this.args.Count() == 0) return "";
            return string.Format( "|{0}| ", string.Join(",", this.args.ToArray()));
        }

        public static LpAstBlock toNode(object[] nodes)
        {
            var blk2 = nodes;
            return new Ast.LpAstBlock(
                LpParser.toNode((object[])blk2[2]).ChildNodes,
                (string[])blk2[0],
                (bool)blk2[1]);
        }
    }
}
