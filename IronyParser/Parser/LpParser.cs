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
            var SimpleExpr = new NonTerminal("SimpleExpr", typeof(Node.SimpleExpr));
            var MulExpr = new NonTerminal("MulExpr", typeof(Node.Expr));
            var AddExpr = new NonTerminal("AddExpr", typeof(Node.Expr));
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            var ShiftExpr = new NonTerminal("ShiftExpr", typeof(Node.Expr));
            var Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));
            var BracketedStmt = new NonTerminal("BracketedStmt", typeof(Node.BracketedStme));
            var Stmts = new NonTerminal("Stmts", typeof(Node.Stmts));

            Symbol.Rule = ":" + Id;
            ArrayItems.Rule = MakeStarRule(ArrayItems, Comma, Stmt);
            Array.Rule = ArrayStart + ArrayItems + ArrayEnd;
            AssocVal.Rule = Stmt + ToTerm("=>") + Stmt;
            Assoc.Rule = MakeStarRule(Assoc, Comma, AssocVal);
            Hash.Rule = ToTerm("{") + Assoc + ToTerm("}");
            Primary.Rule = Num | Str | "true" | "false" | "nl" | Symbol | Id | Array | Hash;
            BracketedStmt.Rule = "(" + Stmt + ")";
            SimpleExpr.Rule = BracketedStmt | Primary;
            MulExpr.Rule = makeChainOperators(new string[] { "*", "/", "%" }, SimpleExpr);
            AddExpr.Rule = makeChainOperators(new string[] { "+", "-" }, MulExpr);
            //ShiftExpr.Rule = makeChainOperators(new string[] { "<<", ">>" }, AddExpr);
            Expr = AddExpr;
            Stmt.Rule = Expr;
            Stmts.Rule = MakeStarRule(Stmts, ToTerm(";"), Stmt);
            // Root = Stmts;
            Root = Primary;
            //static readonly Parser<object[]> SIMPLE_EXP = EXP_VAL.Or(FUNCALL).Or(VARCALL).Or(PRIMARY);

        }

        BnfExpression makeChainOperators(string[] operands, NonTerminal beforeExpr)
        {
            return operands.Select((op) => makeChainOperator(op, beforeExpr)).Aggregate((a, b) => a | b);
        }

        BnfExpression makeChainOperator(string op, NonTerminal beforeExpr)
        {
            return beforeExpr + ToTerm(op) + beforeExpr;
        }
    }
}
