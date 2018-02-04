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
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));

            Primary.Rule = Num | Str;

            KeyTerm Plus = ToTerm("+");

            NonTerminal Expr = new NonTerminal("Expr", typeof(Node.Expr));
            Expr.Rule = Primary + Plus + Primary;

            NonTerminal Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));
            Stmt.Rule = Expr;

            Root = Stmt;
        }
    }
}
