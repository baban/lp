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
            this.block = blk;

            init();
        }

        public void init()
        {
            this.Evaluate = DoEvaluate;
            this.Source = toSource;
        }

        public override Object.LpObject DoEvaluate()
        {
            return lft.DoEvaluate().funcall(
                this.name,
                args.Select( (arg) => arg.DoEvaluate() ).ToArray(),
                ( this.block==null ? null : this.block.DoEvaluate()) );
        }

        public override string toSource()
        {
            return string.Format("{0}.{1}({2}){3}",
                lft.toSource(),
                this.name,
                this.toSourceArgs(),
                this.toSourceBlock() );
        }

        private string toSourceArgs(){
            return string.Join(", ", this.args.Select((o) => o.toSource()).ToArray());
        }

        private string toSourceBlock()
        {
            if (this.block == null) return "";
            return " "+this.block.toSource();
        }
    }
}
