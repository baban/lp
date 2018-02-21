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
            var Comma = ToTerm(",", "Comma");
            var Id = new IdentifierTerminal("identifier");

            var Numeric = new NonTerminal("Primary", typeof(Node.Numeric));
            var Str = new NonTerminal("String", typeof(Node.String));
            var Bool = new NonTerminal("Boolean", typeof(Node.Bool));
            var Nl = new NonTerminal("Nl", typeof(Node.Nl));
            var Symbol = new NonTerminal("Symbol", typeof(Node.Symbol));
            var Array = new NonTerminal("Array", typeof(Node.Array));
            var ArrayItems = new NonTerminal("ArrayItems", typeof(Node.ArrayItems));
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));

            var Expr = new NonTerminal("Expr");
            var Stmt = new NonTerminal("Stmt");

            Numeric.Rule = new NumberLiteral("Number");
            Str.Rule = new StringLiteral("String", "\"");
            Bool.Rule = ToTerm("true") | "false";
            Nl.Rule = ToTerm("nl");
            Symbol.Rule = ":" + Id;
            ArrayItems.Rule = MakeStarRule(ArrayItems, Comma, Numeric);
            Array.Rule = (ToTerm("[") + ArrayItems + ToTerm("]")) | ToTerm("[") + Empty + ToTerm("]");
            Primary.Rule = Numeric | Str | Bool | Nl | Symbol | Array;

            Expr = Primary;
            Stmt = Expr;

            Root = Stmt;
        }
    }
}
