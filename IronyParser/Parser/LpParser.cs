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
            var Minus = ToTerm("-");
            var Mul = ToTerm("*");
            var Div = ToTerm("/");
            var Mod = ToTerm("%");

            var Num = new NumberLiteral("Number");
            var Str = new StringLiteral("String", "\"");
            var Id = new IdentifierTerminal("identifier");
            var Symbol = new NonTerminal("Symbol", typeof(Node.Symbol));
            var Array = new NonTerminal("Array", typeof(Node.Array));
            var ArrayItems = new NonTerminal("ArrayItems", typeof(Node.ArrayItems));
            var AssocVal = new NonTerminal("AssocVal", typeof(Node.AssocVal));
            var Assoc = new NonTerminal("Assoc", typeof(Node.Assoc));
            var Hash = new NonTerminal("Hash", typeof(Node.Hash));
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));
            var MulExpr = new NonTerminal("MulExpr", typeof(Node.Expr));
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            var Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));

            Symbol.Rule = ":" + Id;
            ArrayItems.Rule = MakeStarRule(ArrayItems, Comma, Stmt);
            Array.Rule = ArrayStart + ArrayItems + ArrayEnd;
            AssocVal.Rule = Stmt + ToTerm("=>") + Stmt;
            Assoc.Rule = MakeStarRule(Assoc, Comma, AssocVal);
            Hash.Rule = ToTerm("{") + Assoc + ToTerm("}");
            Primary.Rule = Num | Str | "true" | "false" | "nl" | Symbol | Id | Array | Hash;
            MulExpr.Rule = Primary + Mul + Primary | Primary + Div + Primary | Primary + Mod + Primary | Primary;
            Expr.Rule = MulExpr + Plus + MulExpr | MulExpr + Minus + MulExpr;
            Stmt.Rule = Expr;
            Root = Stmt;
        }
    }
}
