﻿using System;
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
            var content = "1 + 1";
            Console.WriteLine("create");
            ScriptApp app = new ScriptApp(new LanguageData(new Parser.LpGrammer()));
            Console.WriteLine("parse");
            var tree = app.Parser.Parse(content);
            
            string result = (string)app.Evaluate(tree);
            if (result == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(result);
                Console.WriteLine("result: {0}", result);
            }
            Console.WriteLine("Finish");
        }
    }
}
