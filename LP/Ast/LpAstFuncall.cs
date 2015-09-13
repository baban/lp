using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Ast
{
    class LpAstFuncall : LpAstNode
    {
        private string name = null;
        private LpAstNode[] args = null;
        private LpAstNode block = null;

        public LpAstFuncall(string name, LpAstNode[] args, LpAstNode blk)
        {
            this.name = name;
            this.args = args;
            this.block = blk;
            this.Evaluate = DoEvaluate;
            this.Expand = DoExpand;
        }

        public override Object.LpObject DoEvaluate()
        {
            var newArgs = args.Select((arg) => arg.DoEvaluate()).ToArray();
            var newBlock = (this.block == null ? null : this.block.DoEvaluate());

            // get context
            // 取得文脈から関数、macro検索
            var ctx = Util.LpIndexer.loadMethod(this.name);
            if (ctx != null)
            {
                if (ctx.is_macro == true)
                {
                    // macro call
                    var node = ctx.macroexpand(args, this.block);
                    var ret = node.DoEvaluate();
                    return ret;
                }
                else
                {
                    // function call
                    var ret = ctx.execMethod(this.name, ctx, newArgs, newBlock);
                    return ret;
                }
            }
            else
            {
                // 現在のクラスから継承関係を遡ってメソッド呼び出し
                var cls = Util.LpIndexer.last();

                while (false == cls.isMethodExist(name, cls, newArgs, newBlock))
                {
                    cls = cls.superclass;
                    
                    if (cls == null) break;
                }

                if (cls == null)
                {
                    var e = new Error.LpNoMethodError();
                    throw e;

                }
                else {
                    var ret = cls.execMethod(name, cls, newArgs, newBlock);
                    return ret;
                }
            }
        }

        public override LpAstNode DoExpand()
        {
            ChildNodes = ChildNodes.Select((node) => node.Expand()).ToList();

            return this;
        }

        public override string toSource()
        {
            return string.Format("{0}({1}){2}",
                this.name,
                string.Join(", ", args.Select((node) => node.toSource()).ToArray()),
                toSourceBlock());
        }

        private string toSourceBlock()
        {
            if (this.block == null) return "";
            return " " + this.block.toSource();
        }

        public static LpAstFuncall toNode(object[] vals)
        {
            object[] block = vals[2] as object[];
            return new Ast.LpAstFuncall(
                (string)vals[0],
                (Ast.LpAstNode[])((object[])vals[1]).Select((n) => LpParser.toNode((object[])n)).ToArray(),
                (block == null) ? null : LpParser.toNode(block));
        }
    }
}
