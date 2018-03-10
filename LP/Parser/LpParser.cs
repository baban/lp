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
            var ClassName = Id;

            var Numeric = new NonTerminal("Primary", typeof(Node.Numeric));
            var Str = new NonTerminal("String", typeof(Node.String));
            var Bool = new NonTerminal("Boolean", typeof(Node.Bool));
            var Nl = new NonTerminal("Nl", typeof(Node.Nl));
            var Symbol = new NonTerminal("Symbol", typeof(Node.Symbol));
            var Array = new NonTerminal("Array", typeof(Node.Array));
            var ArrayItems = new NonTerminal("ArrayItems", typeof(Node.ArrayItems));
            var AssocVal = new NonTerminal("AssocVal", typeof(Node.AssocVal));
            var Assoc = new NonTerminal("Assoc", typeof(Node.Assoc));
            var Hash = new NonTerminal("Hash", typeof(Node.Hash));
            var Block = new NonTerminal("Block", typeof(Node.Block));
            var Lambda = new NonTerminal("Lambda", typeof(Node.Block));
            var Quote = new NonTerminal("Quote", typeof(Node.Quote));
            var QuasiQuote = new NonTerminal("QuasiQuote", typeof(Node.QuasiQuote));
            var QuestionQuote = new NonTerminal("QuestionQuote", typeof(Node.QuestionQuote));
            var VariableCall = new NonTerminal("VariableCall", typeof(Node.VariableCall));
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));

            var Expr16 = new NonTerminal("Expr16", typeof(Node.LeftUnary));
            var Expr15 = new NonTerminal("Expr15", typeof(Node.RightUnary));
            var Expr14 = new NonTerminal("Expr14", typeof(Node.RightUnary));
            var Expr13 = new NonTerminal("Expr13", typeof(Node.BinExpr));
            var Expr12 = new NonTerminal("Expr12", typeof(Node.Expr));
            var Expr11 = new NonTerminal("Expr11", typeof(Node.Expr));
            var Expr10 = new NonTerminal("Expr10", typeof(Node.Expr));
            var Expr9 = new NonTerminal("Expr9", typeof(Node.Expr));
            var Expr8 = new NonTerminal("Expr8", typeof(Node.Expr));
            var Expr7 = new NonTerminal("Expr7", typeof(Node.Expr));
            var Expr6 = new NonTerminal("Expr6", typeof(Node.Expr));
            var Expr5 = new NonTerminal("Expr5", typeof(Node.Expr));
            var Expr4 = new NonTerminal("Expr4", typeof(Node.Expr));
            var Expr3 = new NonTerminal("Expr3", typeof(Node.Expr));
            var Expr2 = new NonTerminal("Expr2", typeof(Node.Expr));
            var Expr1 = new NonTerminal("Expr1", typeof(Node.Expr));
            var Expr0 = new NonTerminal("Expr0", typeof(Node.Expr));
            var SimpleExpr = new NonTerminal("SimpleExpr", typeof(Node.Expr));
            var BinExpr = new NonTerminal("BinExpr", typeof(Node.BinExpr));
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            var Funcall = new NonTerminal("Funcall", typeof(Node.Funcall));
            var MethodCall = new NonTerminal("MethodCall");

            var BracketedStmt = new NonTerminal("BracketedStmt", typeof(Node.BracketedStmt));
            var IfStmt = new NonTerminal("IfStmt", typeof(Node.IfStmt));
            var DefineFunction = new NonTerminal("DefineFunction", typeof(Node.DefineFunction));
            var DefineClass = new NonTerminal("DefineClass", typeof(Node.DefineClass));
            var Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));

            var Stmts = new NonTerminal("Stmts", typeof(Node.Stmts));
            RegisterBracePair("(", ")");
            RegisterOperators(0, "=");
            RegisterOperators(1, "and", "or");
            RegisterOperators(2, "..", "^..", "..^", "^..^");
            RegisterOperators(3, "||");
            RegisterOperators(4, "&&");
            RegisterOperators(5, "<=>", "===", "==", "!=", "=~", "!~");
            RegisterOperators(6, ">=", ">", "<=", "<");
            RegisterOperators(7, "|");
            RegisterOperators(8, "&");
            RegisterOperators(9, "<<", ">>");
            RegisterOperators(9, "+", "-");
            RegisterOperators(10, "*", "/", "%");
            RegisterOperators(12, "-");
            RegisterOperators(13, "**");
            RegisterOperators(14, "not");
            RegisterOperators(15, "+", "!", "~");
            RegisterOperators(16, "++", "--");
            MarkPunctuation(",", "(", ")");
            //this.Delimiters = "{}[](),:;+-*/%&|^!~<>=";
            this.MarkPunctuation(";", ",", "(", ")", "{", "}", "[", "]", ":");
            this.MarkTransient(Stmt, Primary, Expr);

            Numeric.Rule = new NumberLiteral("Number");
            Str.Rule = new StringLiteral("String", "\"");
            Bool.Rule = ToTerm("true") | "false";
            Nl.Rule = ToTerm("nl");
            Symbol.Rule = ":" + Id;
            ArrayItems.Rule = MakeStarRule(ArrayItems, Comma, Stmt) | Empty;
            Array.Rule = ToTerm("[") + ArrayItems + ToTerm("]");
            AssocVal.Rule = Stmt + ToTerm("=>") + Stmt;
            Assoc.Rule = MakeStarRule(Assoc, Comma, AssocVal) | Empty;
            Hash.Rule = ToTerm("{") + Assoc + ToTerm("}");
            Block.Rule = ToTerm("do") + Stmts + ToTerm("end");
            Lambda.Rule = ToTerm("->") + ToTerm("do") + Stmts + ToTerm("end");
            Quote.Rule = "'" + Stmt;
            QuasiQuote.Rule = "`" + Stmt;
            QuestionQuote.Rule = "?" + Stmt;
            VariableCall.Rule = Id;
            Primary.Rule = Numeric | Str | Bool | Nl | Symbol | Array | Hash | Block | Lambda | Quote | QuasiQuote | QuestionQuote | VariableCall;

            SimpleExpr.Rule = BracketedStmt | Primary;
            BracketedStmt.Rule = "(" + Stmt + ")";
            Expr16.Rule = SimpleExpr | (SimpleExpr + "++") | (SimpleExpr + "--");
            Expr15.Rule = Expr16 | ("+" + Expr16) | ("!" + Expr16) | ("~" + Expr16);
            Expr14.Rule = Expr15 | ToTerm("not") + Expr15;
            Expr13.Rule = Expr14 | makeChainOperators(new string[] { "**" }, Expr14, Expr13);
            Expr12.Rule = Expr13 | "-" + Expr13;
            Expr11.Rule = SimpleExpr | makeChainOperators(new string[] { "*", "/" }, SimpleExpr, Expr11);
            Expr10.Rule = Expr11 | makeChainOperators(new string[] { "+", "-" }, Expr11, Expr10);
            Expr9.Rule = Expr10 | makeChainOperators(new string[] { "<<", ">>" }, Expr10, Expr9);
            Expr8.Rule = Expr9 | makeChainOperators(new string[] { "&" }, Expr9, Expr8);
            Expr7.Rule = Expr8 | makeChainOperators(new string[] { "|" }, Expr8, Expr7);
            Expr6.Rule = Expr7 | makeChainOperators(new string[] { ">=", ">", "<=", "<" }, Expr7, Expr6);
            Expr5.Rule = Expr6 | makeChainOperators(new string[] { "<=>", "===", "==", "!=", "=~", "!~" }, Expr6, Expr5);
            Expr4.Rule = Expr5 | makeChainOperators(new string[] { "&&" }, Expr5, Expr4);
            Expr3.Rule = Expr4 | makeChainOperators(new string[] { "||" }, Expr4, Expr3);
            Expr2.Rule = Expr3 | makeChainOperators(new string[] { "..", "^..", "..^", "^..^" }, Expr3, Expr2);
            Expr1.Rule = Expr2 | makeChainOperators(new string[] { "and", "or" }, Expr2, Expr1);
            Expr0.Rule = Expr1 | makeChainOperators(new string[] { "=" }, VariableCall, Expr1);
            Funcall.Rule = Id + "()";
            MethodCall.Rule = SimpleExpr + "." + Funcall;
            Expr.Rule = Expr0;

            IfStmt.Rule = ToTerm("if(") + Stmt + ToTerm(")") + Stmts + ToTerm("end");
            DefineFunction.Rule = ToTerm("def") + Id + "()" + Stmts + ToTerm("end");
            DefineClass.Rule = ToTerm("class")+ ClassName + ToTerm(";") + Stmts + ToTerm("end");
            Stmt.Rule = DefineClass | DefineFunction | IfStmt | Expr;

            Stmts.Rule = MakeStarRule(Stmts, ToTerm(";"), Stmt);

            Root = Stmts;
        }

        BnfExpression makeChainOperators(string[] operands, NonTerminal beforeExpr, NonTerminal afterExpr)
        {
            return operands.Select((op) => makeChainOperator(op, beforeExpr, afterExpr)).Aggregate((a, b) => a | b);
        }

        BnfExpression makeChainOperator(string op, NonTerminal beforeExpr, NonTerminal afterExpr)
        {
            var BinExpr = new NonTerminal("BinExpr", typeof(Node.BinExpr));
            BinExpr.Rule = beforeExpr + ToTerm(op) + afterExpr;
            return BinExpr;
        }
    }
}
