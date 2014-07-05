using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstStmts : LpAstNode
    {
        public LpAstStmts( List<LpAstNode> nodes ){
            ChildNodes = nodes;
            this.Evaluate = DoEvaluate;
        }

        public override Object.LpObject DoEvaluate()
        {
            Object.LpObject ret = Object.LpNl.initialize();
            ChildNodes.ForEach((node) =>{ ret = node.Evaluate(); });
            return ret;
        }

        public virtual string toSource()
        {
            return string.Join("; ", ChildNodes.Select((node) => node.toSource()));
        }
    }
}
