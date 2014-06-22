using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstMethodCall : LpAstNode
    {
        private string name = null;
        private LpAstNode lft = null;
        private LpAstNode[] args = null;
        private LpAstNode block = null;

        public LpAstMethodCall( string name, LpAstNode lft, LpAstNode[] args, LpAstNode blk ) {
            this.name = name;
            this.lft = lft;
            this.args = args;
            this.block = null;

            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            return lft.DoEvaluate().funcall(this.name, args.Select( (arg) => arg.DoEvaluate() ).ToArray(), null);
        }
    }
}
