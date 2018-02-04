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
            var Plus = ToTerm("+");

            var Num = new NumberLiteral("Number");
            var Str = new StringLiteral("String", "\"");
            var Id = new IdentifierTerminal("identifier");
            var Symbol = new NonTerminal("Symbol", typeof(Node.Symbol));
            var Array = new NonTerminal("Array", typeof(Node.Array));
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            var Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));

            Symbol.Rule = ":" + Id;
            Array.Rule = ArrayStart + Num +  ArrayEnd | ArrayStart + ArrayEnd;
            Primary.Rule = Num | Str | Array | "true" | "false" | "nl" | Symbol | Id;
            Expr.Rule = Primary + Plus + Primary;
            Stmt.Rule = Expr;
            Root = Stmt;
        }
    }
}
