using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using Irony.Interpreter;

namespace IronyParser.Parser
{
    [Language("LpGrammar")]
    public class LpGrammer : InterpretedLanguageGrammar
    {
        public LpGrammer() : base(true)
        {
            var Num = new NumberLiteral("Number");
            var Str = new StringLiteral("String", "\"");
            var Id = new IdentifierTerminal("identifier");
            var Value = new NonTerminal("Value");

            NumberLiteral Number = new NumberLiteral("Number");
            KeyTerm Comma = ToTerm(",");

            NonTerminal Numbers = new NonTerminal("Numbers", typeof(Node.NumbersNode));  /* 変更点(2) */
            Numbers.Rule = Number + Comma + Number;

            Root = Numbers;
            //Value.Rule = Num;
            //Root = Value;
        }
    }
}
