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

        public delegate Object.LpObject EvaluateMethod(bool expand = false);
        public delegate LpAstNode ExpandMethod();
        public EvaluateMethod Evaluate;
        public ExpandMethod Expand;
        public delegate string toSourceMethod(bool expand);
        public toSourceMethod Source;

        public LpAstNode()
        {
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
            this.Source = toSource;
        }

        public virtual Object.LpObject DoEvaluate(bool expand = false)
        {
            var ret = LP.Object.LpNl.initialize();
            foreach (Ast.LpAstNode stmt in ChildNodes)
            {
                ret = stmt.Evaluate(false);
            }
            return ret;
        }

        public virtual LpAstNode DoExpand()
        {
            return this;
        }

        public virtual string toSource( bool expand = false )
        {
            return null;
        }
    }
}
