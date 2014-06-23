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
            Util.LpIndexer.push( Object.LpKernel.initialize() );
            /*
            LpParser.execute("10; 5");
            LpParser.execute("8.(+)(3)");
            LpParser.execute("print(7)"); 
            LpParser.execute("10.(+)(10).(+)(5)");
            LpParser.execute("->() do 10 end");
            LpParser.execute("->() do 10 end.call()");
            LpParser.execute("->(n) do 10; 5.(+)(3) end.call(7)");
            LpParser.execute(":n.(=)(7); n.(+)(5)");
            LpParser.execute(":a.(=)(7); print(a)");
             */
            //LpParser.execute("->(g) do 10 end.call(6)");
            LpParser.execute("->(g) do g.(+)(2) end.call(6)");
            //LpParser.execute("->(g) do print(g) end.call(6)");
            /*
            LpParser.execute("10");
            LpParser.execute("10.(+)(10)");
            LpParser.execute("10.(+)(10).(+)(5).(+)(7).(+)(7).( -)(7).(+)(7)");
            LpParser.execute("6;5");
            //LpParser.execute("-> do 10 end");
            LpParser.execute("->() do 10 end.call()");
             */
            //LpParser.execute("(10).(+)(6)");
            //LpParser.execute("->(n) do print(n) end.bind(:fact);");
            //LpParser.execute("->() do 10 end.bind(:fact); fact()");
            //LpParser.execute("->(n) do 10 end.bind(:fact);fact(10)");
            //LpParser.execute("->(n) do n end.bind(:fact);fact(10)");
            //"->(n) do _if(n.(==)(1),do 1 end,do n.(*)(fact(n.(-)(1))) end) end.bind(:fact); fact(2).display()";
             
            //sysInit("", args, 0);
            //runNode(args);
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
            if (argv.Length == 0)
            {
                showSystemMessage();
                return 0;
            }
            string code = readFile(argv[0]);
            LP.Util.LpIndexer.initialize();
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
            Object.LpKernel.initialize();
            return;
        }
    }
}
