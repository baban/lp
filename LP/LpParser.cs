using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using System.Diagnostics;

namespace LP
{
    /*
     mac(until) do |test,&body|
       `while(?test) &body
     end
     
     until(test) &body
     
     until(i<10) do
       i++;
       print(i);
     end
     */

    // TODO: エラー行表示
    // TODO: メソッド定義
    // TODO: module定義
    // TODO: Block読み出し
    // TODO: インスタンス変数
    // TODO: load & require
    // TODO: マクロのデバッグ
    // TODO: ハッシュ引数
    //
    // TODO: コメントはできるだけあとに残す
    // TODO: 文字列のこれ以上細かいところは後日実装する
    // TODO: マクロの変数をautogemsymする
    // TODO: .Netとの相互運用性の向上
    //\e
    //\s
    //\nnn
    //\C-x
    //\M-x
    //\M-\C-x
    //\x

    // メソッドの呼び出し順序
    // 1.まずブロックを手繰る
    // 2.それで見つからなければ継承関係を手繰る
    // 3. 見つからなければKernelのメソッドを探す
    class LpParser
    {
        public static Ast.LpAstNode toNode( object[] node ) {
            return Parser.BaseParser.toNode(node);
        }

        public static Ast.LpAstNode createNode(string ctx)
        {
            return Parser.BaseParser.createNode(ctx);
        }

        public static Object.LpObject execute(string ctx)
        {
            /*
            Console.WriteLine(ctx);
            var str = Program.Parse(ctx);
            Console.WriteLine(str);
            var pobj = PROGRAM.Parse(str);
            Console.WriteLine(pobj);
            var node = toNode(pobj);
            Console.WriteLine(node);
            */
            var node = Parser.BaseParser.createNode(ctx);
            //var node = createNode(ctx);
            var o = node.Evaluate();

            return o;
        }
    }
}
