using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Interpreter;

namespace IronyParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            var content = "123+456";
            ScriptApp app = new ScriptApp(new LanguageData(new Parser.LpGrammer()));
            string result = (string)app.Evaluate(content);
            if (result == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(result);
                Console.WriteLine("Left: {0}", result);
            }
            Console.WriteLine("Finish");
        }
    }
}
