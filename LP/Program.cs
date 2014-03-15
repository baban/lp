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
            runNode(args);
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
            if (argv.Length == 0) {
                showSystemMessage();
                return 0;
            }
            Console.WriteLine(DateTime.UtcNow.ToString());
            string code = readFile(argv[0]);
            LP.Util.LpIndexer.initialize();
            LpParser.execute(code);
            Console.WriteLine(DateTime.UtcNow.ToString());
            return 0;
        }

        static void showSystemMessage()
        {
            var message = " LP version 0.1 ";
            Console.WriteLine( message );
        }

        static string readFile( string filename ){

            StreamReader sr = new StreamReader(filename);
            string line = "";
            string txt = "";

            while (line != null)
            {
                line = sr.ReadLine();
                if (line != null)
                    txt += line + "\n";
            }
            sr.Close();

            return txt;
        }

        static void initEnv(){
            Object.LpKernel.initialize();
            return;
        }
    }
}
