using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstFuncall : LpAstNode
    {
        private string name = null;
        private LpAstNode[] args = null;
        private LpAstNode block = null;

        public LpAstFuncall(string name, LpAstNode[] args, LpAstNode blk)
        {
            this.name = name;
            this.args = args;
            this.block = blk;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            var lft = Util.LpIndexer.last();
            return lft.funcall(
                this.name,
                args.Select((arg) => arg.DoEvaluate()).ToArray(),
                (this.block == null ? null : this.block.DoEvaluate()));
        }
    }
}
