using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstNode
    {
        public LpAstNode Parent = null;
        public List<LpAstNode> ChildNodes = new List<LpAstNode>();
        public Object.LpObject Leaf = null;

        public delegate Object.LpObject EvaluateMethod();
        public EvaluateMethod Evaluate;

        public LpAstNode()
        {
            this.Evaluate = DoEvaluate;
        }

        public virtual Object.LpObject DoEvaluate()
        {
            return null;
        }
    }
}
