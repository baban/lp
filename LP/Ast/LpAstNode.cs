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
        public delegate LpAstNode ExpandMethod();
        public ExpandMethod Expand;
        public delegate string toSourceMethod();
        public toSourceMethod Source;

        public LpAstNode()
        {
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
            this.Source = toSource;
        }

        public virtual Object.LpObject DoEvaluate()
        {
            var ret = LP.Object.LpNl.initialize();
            foreach (Ast.LpAstNode stmt in ChildNodes)
            {
                ret = stmt.Evaluate();
            }
            return ret;
        }

        public virtual LpAstNode DoExpand()
        {
            return this;
        }

        public virtual string toSource()
        {
            return null;
        }
    }
}
