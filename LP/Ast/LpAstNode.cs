﻿using System;
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
        public delegate string toSourceMethod();
        public toSourceMethod Source;
        public delegate string expandMethod();
        public expandMethod Expand;

        public LpAstNode()
        {
            this.Evaluate = DoEvaluate;
            this.Source = toSource;
            this.Expand = expand;
        }

        public virtual Object.LpObject DoEvaluate()
        {
            return null;
        }

        public virtual string toSource()
        {
            return null;
        }

        public virtual string expand()
        {
            return toSource();
        }
    }
}
