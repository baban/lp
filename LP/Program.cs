using System;
using System.Linq;
using System.Text;
using System.IO;
using Irony.Parsing;
using Irony.Interpreter;
using System.Diagnostics;

namespace LP
{
    // TODO: エラー時にスタックトレースを出す
    // TODO: .Netライブラリの相互運用
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
    class Program
    {
        static void Main(string[] args)
        {
            //runTestCode();
            //return;

            if (args.Length == 0) {
                sysInit("", args, 0);
                consoleReadFile();
            }
            var options = new Options();
            //コマンドライン引数を解析する
            bool isSuccess = CommandLine.Parser.Default.ParseArguments(args, options);
            if (isSuccess)
            {
                if (options.Verbose)
                {
                    printVersion();
                    return;
                }

                if ( options.Evaluate != null )
                {
                    sysInit("", args, 0);
                    Console.WriteLine("initialize");
                    var parser = new Parser.LpGrammer();
                    var language = new LanguageData(parser);
                    ScriptApp app = new ScriptApp(language);
                    Console.WriteLine("parse");
                    return;
                }

                if (options.InputFiles.Count>0) {
                    sysInit("", args, 0);
                    runNode(options.InputFiles.ToArray());
                }
            }
        }

        static long sysInit(string args, string[] argv, int enviroment)
        {
            initEnv();
            return 0;
        }

        static long init(string args, string[] argv, int enviroment)
        {
            return 0;
        }

        static long runTestCode()
        {
            /*
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            */
            //string code = readFile(argv[0]);
            //string code = "/* 111 */ 2";
            //string code = "1";
            //string code = ":aaaa";
            //string code = "\"Hello\"";
            //string code = "true";
            //string code = "nl";
            //string code = "/regex/";
            //string code = "[]";
            //string code = "{}";
            //string code = "do |a| end";
            //string code = "-> do |a| end";
            //string code = "a=1";
            //string code = "a=1; a";
            //string code = "let a=1; a";
            //string code = "b? = 2; b?";
            //string code = "@a = 3; @a";
            //string code = "@@a = 4; @@a";
            //string code = "let a";
            //string code = "1; 2; 3";
            //string code = "a='(1+2); ?a";
            //string code = "a='(1+2); `(1+3)";
            //string code = "a='(2+3); `(1+?a)";
            //string code = "1+a";
            //string code = "1+1";
            //string code = "2*3";
            //string code = "!true";
            //string code = "1+2*3+4";
            //string code = "1.to_s()";
            //string code = "Console";
            string code = "Console.WriteLine(\"Hello,World\")";
            //string code = "def hoge() end";
            //string code = "def hoge(a) 1; 2; 3 end";
            //string code = "def hoge(a, b) 1; 2; 3 end";
            //string code = "public def hoge(a) 1; 2; 3 end";
            //string code = "abc=1+5*5; abc";
            //string code = "def bbb(a,b,c) 1; 2; c end; bbb(1,2,3)";
            //string code = "class Aaa; 1;2;3 end";
            //string code = "public class A; 1;2;3 end";
            //string code = "public class A < B; 1;2;3 end";
            //string code = "module Aaa; 1;2;3 end";
            //string code = "public module Aaa; 1;2;3 end";
            //string code = "if true; 1 end";
            //string code = "if false; 1 else 2 end";
            //string code = "if false; 1 elsif true; 2 end";
            //string code = "case 1; end";
            //string code = "case false; else 1 end";
            //string code = "case 1; when 1; 3 end";
            Debug.WriteLine("initialize");
            var parser = new Parser.LpGrammer();
            //Console.WriteLine("initialize parser");
            var language = new LanguageData(parser);
            //Console.WriteLine("initialize language");
            ScriptApp app = new ScriptApp(language);
            Debug.WriteLine("parse");
            var tree = app.Parser.Parse(code);
            //Console.WriteLine("tree");
            //Console.WriteLine(tree);

            /*
            if (tree.HasErrors())
            {
                Console.WriteLine(tree.ParserMessages.First().Message);
                Console.WriteLine(tree.FileName);
                Console.WriteLine(tree.ParserMessages.First().Location);
                return 0;
            }
            */


            Debug.WriteLine("evaluate");

            Object.LpObject result = null;
            try
            {
                result = (Object.LpObject)app.Evaluate(tree);
                if (result == null)
                {
                    Debug.WriteLine("null");
                }
                else
                {
                    Console.WriteLine(result);
                    Console.WriteLine("result: {0}", result);
                    result.funcall("display", new Object.LpObject[] { }, null);
                }
            }
            catch (Error.LpError e)
            {
                Console.WriteLine(e.ToString());
            }
            Debug.WriteLine("Finish");
            /*
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.WriteLine("benckmark:end");
            */
            return 0;
        }

        static long runNode(string[] argv)
        {
            string fileName = argv[0];
            string code = readFile(fileName);

            var parser = new Parser.LpGrammer();
            var language = new LanguageData(parser);
            ScriptApp app = new ScriptApp(language);
            var tree = app.Parser.Parse(code);

            Object.LpObject result = null;
            try
            {
                result = (Object.LpObject)app.Evaluate(tree);
                if (result == null)
                {
                    Debug.WriteLine("null");
                }
                else
                {
                    Console.WriteLine(result);
                    Console.WriteLine("result: {0}", result);
                    result.funcall("display", new Object.LpObject[] { }, null);
                }
            }
            catch (Error.LpError e)
            {
                Console.WriteLine(e.ToString());
            }
            Debug.WriteLine("Finish");

            return 0;
        }

        static void consoleReadFile()
        {
            printVersion();
            // Console.WriteLine("[GCC 4.2.1 Compatible Apple LLVM 5.0 (clang-500.0.68)] on darwin");
            // Console.WriteLine("[Type 'help' 'copyright' 'credits' or 'licence' for more information");
            Console.WriteLine("initialize");
            var parser = new Parser.LpGrammer();
            //Console.WriteLine("initialize parser");
            var language = new LanguageData(parser);
            //Console.WriteLine("initialize language");
            ScriptApp app = new ScriptApp(language);
            Console.WriteLine("parse");

            string line = null;

            do {
                Console.Write(" >> ");
                line = Console.ReadLine();
                try {
                    var tree = app.Parser.Parse(line);
                    if (tree == null)
                    {
                        Debug.WriteLine("parse error");
                    } else
                    {
                        Object.LpObject result = (Object.LpObject)app.Evaluate(tree);
                        if (result == null)
                        {
                            Debug.WriteLine("null");
                        }
                        else
                        {
                            result.funcall("display", new Object.LpObject[] { }, null);
                        }
                    }
                }
                catch ( Error.LpError e ){
                    printError( e );
                }
            } while (true);
        }

        static void printError( Error.LpError e ) {
            Console.WriteLine("Message");
            Console.WriteLine(e.Message);
            Console.WriteLine("------------------------------------------");
            Console.WriteLine(string.Join("", e.BackTrace.Select((o) => { return o.ToString() + "\n"; })));
            Console.WriteLine("------------------------------------------");
            e.BackTrace.Clear();
            Console.WriteLine(e.StackTrace);
        }

        static void printException(Exception e)
        {
            Console.WriteLine("Message");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }

        static string readFile( string filename ){
            if (!File.Exists(filename))
                return null;

            StreamReader sr = new StreamReader(filename, Encoding.GetEncoding("UTF-8"));
            var str = sr.ReadToEnd();
            sr.Close();
            return str;
        }

        static void initEnv(){
            initializeBuiltInClasses();
            initializeMacros();
            return;
        }

        static void initializeBuiltInClasses()
        {
            Object.LpArray.initialize();
            Object.LpBlock.initialize();
            Object.LpBool.initialize();
            Object.LpClass.initialize();
            Object.LpFile.initialize();
            Object.LpHash.initialize();
            Object.LpKernel.initialize();
            Object.LpLambda.initialize();
            Object.LpMethod.initialize();
            Object.LpModule.initialize();
            Object.LpNl.initialize();
            Object.LpNumeric.initialize();
            Object.LpObject.initialize();
            Object.LpQuote.initialize();
            Object.LpString.initialize();
            Object.LpSymbol.initialize();
            Object.LpMacro.initialize();
        }

        static void initializeMacros() {
            //string untilMacro = "def mac(name,func) name=(`10); end";
            //LpParser.execute(untilMacro);
        }

        private static void printVersion()
        {
            Console.WriteLine(string.Format("LP version {0}", "0.1"));
        }
    }
}
