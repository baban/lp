﻿using System;
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
        }

        public override Object.LpObject DoEvaluate(bool expand = false)
        {
            // 文脈取得
            // 取得文脈から関数、macro検索

            // マクロ実行
            var macro = Util.LpIndexer.loadmacro(this.name);
            if (macro != null)
            {
                // macro call
                var node = macro.macroexpand(args, this.block);
                return node.DoEvaluate();
            }

            // 関数呼び出し実行
            var ctx = Util.LpIndexer.last();
            // function call
            return ctx.funcall(
                this.name,
                args.Select((arg) => arg.DoEvaluate()).ToArray(),
                (this.block == null ? null : this.block.DoEvaluate()) );
        }

        public override string toSource( bool expand=false )
        {
            return string.Format("{0}({1}){2}",
                this.name,
                string.Join(", ", args.Select((node) => node.toSource(expand)).ToArray()),
                toSourceBlock(expand));
        }

        private string toSourceBlock(bool expand )
        {
            if (this.block == null) return "";
            return " " + this.block.toSource(expand);
        }

        public static LpAstFuncall toNode(object[] vals)
        {
            object[] blkf = vals[2] as object[];
            return new Ast.LpAstFuncall(
                (string)vals[0],
                (Ast.LpAstNode[])((object[])vals[1]).Select((n) => LpParser.toNode((object[])n)).ToArray(),
                ((blkf == null) ? null : LpParser.toNode(blkf)));
        }
    }
}
