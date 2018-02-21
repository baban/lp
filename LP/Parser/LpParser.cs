﻿using System;
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
            var Numeric = new NonTerminal("Primary", typeof(Node.Numeric));
            Numeric.Rule = new NumberLiteral("Number");
            var Str = new NonTerminal("String", typeof(Node.String));
            Str.Rule = new StringLiteral("String", "\"");
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));
            Primary.Rule = Numeric | Str;
            Root = Primary;
        }
    }
}
