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
        private LpAstNode arg;

        public LpAstMethodCall( string name, LpAstNode lft, LpAstNode arg ) {
            this.name = name;
            this.lft = lft;
            this.arg = arg;

            this.Evaluate = DoEvaluate;
        }

        public LpAstMethodCall Init() {
            return this;
        }

        public virtual Object.LpObject DoEvaluate()
        {
            Console.WriteLine("AstNode");
            return lft.DoEvaluate().funcall(this.name, new Object.LpObject[] { arg.DoEvaluate() }, null);
        }
    }
}
