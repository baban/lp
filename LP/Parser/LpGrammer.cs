using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony.Interpreter;

namespace LP.Parser
{
    [Language("LpGrammar")]
    public class LpGrammer : InterpretedLanguageGrammar
    {
        public LpGrammer() : base(true)
        {
            var Num = new NumberLiteral("Number");
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));
            Primary.Rule = Num;
            Root = Primary;
        }
    }
}
