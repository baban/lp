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
            var Comma = ToTerm(",", "Comma");
            var Id = new IdentifierTerminal("identifier");

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
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));

            var Expr16 = new NonTerminal("Expr16", typeof(Node.LeftUnary));
            var Expr15 = new NonTerminal("Expr15", typeof(Node.RightUnary));
            var Expr14 = new NonTerminal("Expr14", typeof(Node.RightUnary));
            var Expr13 = new NonTerminal("Expr13", typeof(Node.BinExpr));
            var Expr12 = new NonTerminal("Expr12", typeof(Node.RightUnary));
            var Expr11 = new NonTerminal("Expr11", typeof(Node.BinExpr));
            var Expr10 = new NonTerminal("Expr10", typeof(Node.BinExpr));
            var Expr9 = new NonTerminal("Expr9", typeof(Node.BinExpr));
            var Expr8 = new NonTerminal("Expr8", typeof(Node.BinExpr));
            var Expr7 = new NonTerminal("Expr7", typeof(Node.BinExpr));
            var Expr6 = new NonTerminal("Expr6", typeof(Node.BinExpr));
            var Expr5 = new NonTerminal("Expr5", typeof(Node.BinExpr));
            var Expr4 = new NonTerminal("Expr4", typeof(Node.BinExpr));
            var Expr3 = new NonTerminal("Expr3", typeof(Node.BinExpr));
            var Expr2 = new NonTerminal("Expr2", typeof(Node.BinExpr));
            var Expr1 = new NonTerminal("Expr1", typeof(Node.BinExpr));
            var Expr0 = new NonTerminal("Expr0", typeof(Node.BinExpr));
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));

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
            RegisterOperators(10, "+", "-");
            RegisterOperators(11, "*", "/", "%");
            RegisterOperators(12, "-");
            RegisterOperators(13, "**");
            RegisterOperators(14, "not");
            RegisterOperators(15, "+", "!", "~");
            RegisterOperators(16, "++", "--");
            MarkPunctuation(",", "(", ")");
            //this.Delimiters = "{}[](),:;+-*/%&|^!~<>=";
            this.MarkPunctuation(";", ",", "(", ")", "{", "}", "[", "]", ":");

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
            Primary.Rule = Numeric | Str | Bool | Nl | Symbol | Array | Hash | Block;

            BracketedStmt.Rule = "(" + Stmt + ")";
            Expr16.Rule = (Expr + "++") | (Expr + "--");
            Expr15.Rule = ("+" + Expr) | ("!" + Expr) | ("~" + Expr);
            Expr14.Rule = ToTerm("not") + Expr;
            Expr13.Rule = makeChainOperators(new string[] { "**" }, Expr);
            Expr12.Rule = "-" + Expr;
            Expr11.Rule = makeChainOperators(new string[] { "*", "/", "%" }, Expr);
            Expr10.Rule = makeChainOperators(new string[] { "+", "-" }, Expr);
            Expr9.Rule = makeChainOperators(new string[] { "<<", ">>" }, Expr);
            Expr8.Rule = makeChainOperators(new string[] { "&" }, Expr);
            Expr7.Rule = makeChainOperators(new string[] { "|" }, Expr);
            Expr6.Rule = makeChainOperators(new string[] { ">=", ">", "<=", "<" }, Expr);
            Expr5.Rule = makeChainOperators(new string[] { "<=>", "===", "==", "!=", "=~", "!~" }, Expr);
            Expr4.Rule = makeChainOperators(new string[] { "&&" }, Expr);
            Expr3.Rule = makeChainOperators(new string[] { "||" }, Expr);
            Expr2.Rule = makeChainOperators(new string[] { "..", "^..", "..^", "^..^" }, Expr);
            Expr1.Rule = makeChainOperators(new string[] { "and", "or" }, Expr);
            Expr0.Rule = makeChainOperators(new string[] { "=" }, Expr);
            Expr.Rule = BracketedStmt | Expr16 | Expr15 | Expr14 | Expr13 | Expr12 | Expr11 | Expr10 | Expr9 | Expr8 | Expr7 | Expr6 | Expr5 | Expr4 | Expr3 | Expr2 | Expr1 | Primary;

            IfStmt.Rule = ToTerm("if(") + Stmt + ToTerm(")") + Stmts + ToTerm("end");
            DefineFunction.Rule = ToTerm("def") + Id + "()" + Stmts + ToTerm("end");
            DefineClass.Rule = ToTerm("class")+ Id + Stmts + ToTerm("end");
            Stmt.Rule = DefineClass | DefineFunction | IfStmt | Expr;

            Stmts.Rule = MakeStarRule(Stmts, ToTerm(";"), Stmt);

            Root = Stmts;
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
