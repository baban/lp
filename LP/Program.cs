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
                showSystemMessage();
                return;
            }

            string code = readFile( args[0] );
            LpParser.execute( code );
        }

        static void showSystemMessage() {
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
