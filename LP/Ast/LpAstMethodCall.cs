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
        static Stack<string> programStack = new Stack<string>();


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
            this.Expand = DoExpand;
            this.Source = toSource;
        }

        public override Object.LpObject DoEvaluate()
        {
            programStack.Push(this.name);

            try
            {
                var ret = lft.DoEvaluate().funcall(
                    this.name,
                    args.Select((arg) => arg.DoEvaluate()).ToArray(),
                    (this.block == null ? null : this.block.DoEvaluate()));
                programStack.Pop();
                return ret;
            }
            catch( Error.LpError e )
            {
                e.BackTrace = programStack;
                throw e;
            }
        }

        public override LpAstNode DoExpand()
        {
            lft = lft.DoExpand();
            args = args.Select((arg) => arg.DoExpand()).ToArray();
            if( block != null )
                block = block.DoExpand();

            return this;
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

        public static LpAstMethodCall toNode( object[] nodes ) {
            var vals = nodes;
            object[] block = vals[3] as object[];
            
            return new Ast.LpAstMethodCall(
                (string)vals[0],
                LpParser.toNode( (object[])vals[1] ),
                (Ast.LpAstNode[])((object[])vals[2]).Select((n) => LpParser.toNode((object[])n)).ToArray(),
                LpParser.toNode(block));
        }
    }
}
