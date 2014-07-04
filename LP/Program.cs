using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LP
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
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

                if ( options.Evaluate != "" )
                {
                    printVersion();
                    Console.WriteLine( options.Evaluate );
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

        // TODO; start program
        static long runNode(string[] argv)
        {
            /*
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            */
            string code = readFile(argv[0]);

            try
            {
                LpParser.execute(code);
            }
            catch (Error.LpError e)
            {
                Console.WriteLine(e.ToString());
            }
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
                    LpParser.execute(line).funcall("display",null,null);
                } catch( Error.LpError e ){
                    printError( e );
                } catch ( Sprache.ParseException e ) {
                    printError( e );
                }
            } while (true);
        }

        static void printError( Exception e ) {
            Console.WriteLine("Message");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }

        static string readFile( string filename ){
            if (!System.IO.File.Exists(filename))
                return null;

            StringBuilder strBuff = new StringBuilder();
            System.IO.StreamReader sr = null;
            sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8"));
            while (sr.Peek() >= 0)
            {
                 strBuff.Append( sr.ReadLine() );
                 strBuff.Append("\n");
            }
            sr.Close();
            return strBuff.ToString();
        }

        static void initEnv(){
            LP.Util.LpIndexer.initialize();
            Util.LpIndexer.push(Object.LpKernel.initialize());
            initializeBuiltInClasses();
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
        }

        private static void printVersion()
        {
            Console.WriteLine(string.Format("LP version {0}", 0.1));
        }
    }
}
