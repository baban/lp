using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Irony.Parsing;
using Irony.Interpreter;

namespace LP
{
    class Program
    {
        static void Main(string[] args)
        {
            runNode(new string []{ });
            return;

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
                    //LpParser.execute(options.Evaluate);
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

        static long runNode(string[] argv)
        {
            /*
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            */
            //string code = readFile(argv[0]);
            //string code = "def hoge() 1; 2; 3 end";
            string code = "class Hoge def hoge() 1; 2; 3 end end";
            Console.WriteLine("initialize");
            ScriptApp app = new ScriptApp(new LanguageData(new Parser.LpGrammer()));
            Console.WriteLine("parse");
            var tree = app.Parser.Parse(code);
            Console.WriteLine("evaluate");
            Object.LpObject result = (Object.LpObject)app.Evaluate(tree);
            if (result == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(result);
                Console.WriteLine("result: {0}", result);
                result.funcall("display", new Object.LpObject[] { }, null);
            }
            Console.WriteLine("Finish");
            /*
            try
            {
                LpParser.execute(code);
            }
            catch (Error.LpError e)
            {
                Console.WriteLine(e.ToString());
            }
            */
            /*
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.WriteLine("benckmark:end");
            */
            return 0;
        }

        static void consoleReadFile()
        {
            printVersion();
            //Console.WriteLine("[GCC 4.2.1 Compatible Apple LLVM 5.0 (clang-500.0.68)] on darwin");
            //Console.WriteLine("[Type 'help' 'copyright' 'credits' or 'licence' for more information");

            string line = null;

            do {
                Console.Write(" >> ");
                line = Console.ReadLine();
                try {
                    LpParser.execute(line).funcall("inspect",null,null).funcall("display",null,null);
                } catch( Error.LpError e ){
                    printError( e );
                } catch ( Sprache.ParseException e ) {
                    printException(e);
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
            if (!System.IO.File.Exists(filename))
                return null;

            System.IO.StreamReader sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8"));
            var str = sr.ReadToEnd();
            sr.Close();
            return str;
        }

        static void initEnv(){
            LP.Util.LpIndexer.initialize();
            Util.LpIndexer.push(Object.LpKernel.initialize());
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
            //Object.LpQuote.initialize();
            Object.LpString.initialize();
            Object.LpSymbol.initialize();
            //Object.LpMacro.initialize();
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
