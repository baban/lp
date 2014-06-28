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
            sysInit("", args, 0);

            initEnv();

            if (args.Length==0)
            {
                consoleReadFile();
                return;
            }
            else
            {
                runNode(args);
                return;
            }
        }

        // parse command line options
        // long argc, char **argv, struct cmdline_options *opt, int envopt
        static long procOptions( string args, string[] argv, int enviroment ) {
            return 0;
        }

        //  TODO: initialize system
        static long sysInit(string args, string[] argv, int enviroment)
        {
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

        static void showSystemMessage()
        {
            var message = " LP version 0.1 ";
            Console.WriteLine( message );
        }

        static void consoleReadFile()
        {
            Console.WriteLine("LP version {0}", 0.1);
            //Console.WriteLine("[GCC 4.2.1 Compatible Apple LLVM 5.0 (clang-500.0.68)] on darwin");
            //Console.WriteLine("[Type 'help' 'copyright' 'credits' or 'licence' for more information");

            string code = "";
            string line = null;

            do {
                line = Console.ReadLine();
                LpParser.execute(line);
            } while (true);
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
    }
}
