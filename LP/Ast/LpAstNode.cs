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
        public EvaluateMethod Evaluate;
        public delegate string toSourceMethod( bool expand );
        public toSourceMethod Source;

        public LpAstNode()
        {
            this.Evaluate = DoEvaluate;
            this.Source = toSource;
        }

        public virtual Object.LpObject DoEvaluate(bool expand = false)
        {
            return null;
        }

        public virtual string toSource( bool expand = false )
        {
            return null;
        }
    }
}
