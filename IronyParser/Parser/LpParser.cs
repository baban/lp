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
            var ArrayStart = ToTerm("[");
            var ArrayEnd   = ToTerm("]");
            var Comma = ToTerm(",", "Comma");
            var CommaOpt = new NonTerminal("CommaOpt", Empty | Comma);
            var CommasOpt = new NonTerminal("CommasOpt");
            CommasOpt.Rule = MakeStarRule(CommaOpt, null, Comma);
            var Plus = ToTerm("+");

            var Num = new NumberLiteral("Number");
            var Str = new StringLiteral("String", "\"");
            var Id = new IdentifierTerminal("identifier");
            var Symbol = new NonTerminal("Symbol", typeof(Node.Symbol));
            var Array = new NonTerminal("Array", typeof(Node.Array));
            var ArrayItems = new NonTerminal("ArrayItems", typeof(Node.ArrayItems));
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            var Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));

            Symbol.Rule = ":" + Id;
            ArrayItems.Rule = MakeStarRule(ArrayItems, Comma, Stmt);
            Array.Rule = ArrayStart + ArrayItems + ArrayEnd;
            Primary.Rule = Num | Str | "true" | "false" | "nl" | Symbol | Id | Array;
            Expr.Rule = Primary + Plus + Primary;
            Stmt.Rule = Expr;
            Root = Stmt;
        }
    }
}
