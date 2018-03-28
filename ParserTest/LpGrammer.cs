using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Interpreter;

namespace ParserTest
{
    [Language("LpGrammar v1")]
    public class LpGrammer : InterpretedLanguageGrammar
    {
        public LpGrammer() : base(true)
        {
            NumberLiteral Number = new NumberLiteral("Number");
            KeyTerm Comma = ToTerm(",");

            NonTerminal Numbers = new NonTerminal("Numbers");
            Numbers.Rule = Number + Comma + Number;

            Root = Numbers;
        }
    }
}