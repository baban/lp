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
        NonTerminal Primary;
        NonTerminal Expr;
        NonTerminal Stmt;
        NonTerminal Stmts;
        enum OperandType { CHARIN_OPERATOR, OPERAND, LEFT_UNARY, RIGHT_UNARY };

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
            Primary = new NonTerminal("Primary", typeof(Node.Primary));

            var SimpleExpr = new NonTerminal("SimpleExpr", typeof(Node.Expr));
            var Funcall = new NonTerminal("Funcall", typeof(Node.Funcall));
            var MethodCall = new NonTerminal("MethodCall");
            Expr = new NonTerminal("Expr", typeof(Node.Expr));

            var IfStmt = new NonTerminal("IfStmt", typeof(Node.IfStmt));
            var DefineFunction = new NonTerminal("DefineFunction", typeof(Node.DefineFunction));
            var DefineClass = new NonTerminal("DefineClass", typeof(Node.DefineClass));
            Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));

            Stmts = new NonTerminal("Stmts", typeof(Node.Stmts));
            RegisterBracePair("(", ")");
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
            Quote.Rule = "'" + SimpleExpr;
            QuasiQuote.Rule = "`" + SimpleExpr;
            QuestionQuote.Rule = "?" + SimpleExpr;
            VariableCall.Rule = Id;
            Primary.Rule = Numeric | Str | Bool | Nl | Symbol | Array | Hash | Block | Lambda | Quote | QuasiQuote | QuestionQuote | VariableCall;

            Funcall.Rule = Id + "()";
            MethodCall.Rule = SimpleExpr + "." + Funcall;
            SimpleExpr.Rule = "(" + Stmt + ")" | MethodCall | Funcall | Primary;
            var Expr16 = makeExpr(OperandType.LEFT_UNARY, 16, new string[] { "++", "--" }, SimpleExpr);
            var Expr15 = makeExpr(OperandType.RIGHT_UNARY, 11, new string[] { "+", "!", "~" }, Expr16);
            var Expr14 = makeExpr(OperandType.RIGHT_UNARY, 11, new string[] { "not" }, Expr15);
            var Expr13 = makeExpr(OperandType.CHARIN_OPERATOR, 11, new string[] { "**" }, Expr14);
            var Expr12 = makeExpr(OperandType.RIGHT_UNARY, 12, new string[] { "-" }, Expr13);
            var Expr11 = makeExpr(OperandType.CHARIN_OPERATOR, 11, new string[] { "*", "/", "%" }, Expr12);
            var Expr10 = makeExpr(OperandType.CHARIN_OPERATOR, 10, new string[] { "+", "-" }, Expr11);
            var Expr9 = makeExpr(OperandType.CHARIN_OPERATOR, 9, new string[] { "<<", ">>" }, Expr10);
            var Expr8 = makeExpr(OperandType.CHARIN_OPERATOR, 8, new string[] { "&" }, Expr9);
            var Expr7 = makeExpr(OperandType.CHARIN_OPERATOR, 7, new string[] { "|" }, Expr8);
            var Expr6 = makeExpr(OperandType.CHARIN_OPERATOR, 6, new string[] { ">=", ">", "<=", "<" }, Expr7);
            var Expr5 = makeExpr(OperandType.CHARIN_OPERATOR, 5, new string[] { "<=>", "===", "==", "!=", "=~", "!~" }, Expr6);
            var Expr4 = makeExpr(OperandType.CHARIN_OPERATOR, 4, new string[] { "&&" }, Expr5);
            var Expr3 = makeExpr(OperandType.CHARIN_OPERATOR, 3, new string[] { "||" }, Expr4);
            var Expr2 = makeExpr(OperandType.CHARIN_OPERATOR, 2, new string[] { "..", "^..", "..^", "^..^" }, Expr3);
            var Expr1 = makeExpr(OperandType.CHARIN_OPERATOR, 1, new string[] { "and", "or" }, Expr2);
            var Expr0 = new NonTerminal("Expr0", typeof(Node.Expr));
            Expr0.Rule = Expr1 | makeChainOperators(new string[] { "=" }, VariableCall, Expr1);
            RegisterOperators(0, "=");

            Expr.Rule = Expr0;

            IfStmt.Rule = ToTerm("if(") + Stmt + ToTerm(")") + Stmts + ToTerm("end");
            DefineFunction.Rule = ToTerm("def") + Id + "()" + Stmts + ToTerm("end");
            DefineClass.Rule = ToTerm("class")+ ClassName + ToTerm(";") + Stmts + ToTerm("end");
            Stmt.Rule = DefineClass | DefineFunction | IfStmt | Expr;

            Stmts.Rule = MakeStarRule(Stmts, ToTerm(";"), Stmt);

            Root = Stmts;
        }

        NonTerminal makeExpr(OperandType type, int precedence, string[] operands, NonTerminal primaryExpr)
        {
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            switch (type)
            {
                case OperandType.CHARIN_OPERATOR:
                    Expr.Rule = primaryExpr | makeChainOperators(operands, Expr, primaryExpr);
                    break;
                case OperandType.LEFT_UNARY:
                    Expr.Rule = primaryExpr | makeLeftUnaryOperators(operands, primaryExpr);
                    break;
                case OperandType.RIGHT_UNARY:
                    Expr.Rule = primaryExpr | makeRightUnaryOperators(operands, primaryExpr);
                    break;
            }
            RegisterOperators(precedence, operands);

            return Expr;
        }

        BnfExpression makeLeftUnaryOperators(string[] operands, NonTerminal beforeExpr)
        {
            return operands.Select((op) => makeLeftUnaryOperator(op, beforeExpr)).Aggregate((a, b) => a | b);
        }

        BnfExpression makeLeftUnaryOperator(string op, NonTerminal beforeExpr)
        {
            var Expr = new NonTerminal("LeftUnaryExpr", typeof(Node.LeftUnary));
            Expr.Rule = beforeExpr + op;
            return Expr;
        }

        BnfExpression makeRightUnaryOperators(string[] operands, NonTerminal afterExpr)
        {
            return operands.Select((op) => makeRightUnaryOperator(op, afterExpr)).Aggregate((a, b) => a | b);
        }

        BnfExpression makeRightUnaryOperator(string op, NonTerminal afterExpr)
        {
            var Expr = new NonTerminal("RightUnaryExpr", typeof(Node.RightUnary));
            Expr.Rule = op + afterExpr;
            return Expr;
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
