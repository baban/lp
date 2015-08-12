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
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            Object.LpObject ret = Object.LpNl.initialize();
            ChildNodes.ForEach((node) => { ret = node.Evaluate(expand); });
            return ret;
        }

        public override LpAstNode DoExpand()
        {
            ChildNodes = ChildNodes.Select((node) => node.Expand()).ToList();

            return this;
        }

        public override string toSource(bool expand = false)
        {
            return string.Join("; ", ChildNodes.Select((node) => node.toSource(expand)));
        }

        public static LpAstStmts toNode( List<object[]> nodes ) {
            var stmts = nodes.Select((node) => LpParser.toNode(node)).ToList();
            return new Ast.LpAstStmts(stmts);
        }
    }
}
