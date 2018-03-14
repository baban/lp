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
        enum OperandType { CHARIN_OPERATOR, OPERAND, LEFT_UNARY, RIGHT_UNARY };
        static readonly List<object[]> operandTable = new List<object[]> {
            new object[]{ OperandType.RIGHT_UNARY, new string[]{ "++", "--" } },
            //new object[]{ OperandType.LEFT_UNARY,             new string[]{ "+", "!", "~" } },
            new object[]{ OperandType.LEFT_UNARY,             new string[]{ "!", "~" } },
            new object[]{ OperandType.LEFT_UNARY,          new string[]{ "not" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "**" } },
            new object[]{ OperandType.LEFT_UNARY,             new string[]{ "-" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "*","/", "%" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "+","-" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "<<",">>" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "&" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "|" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ ">=", ">", "<=", "<" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "<=>", "===", "==", "!=", "=~", "!~" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "&&" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "||" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "..", "^..", "..^", "^..^" } },
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "and", "or" } }
        };

        public LpGrammer() : base(true)
        {
            var Comma = ToTerm(",", "Comma");
            var Semi = ToTerm(";", "Semi");
            var Term = Semi | "\n";
            var Id = new IdentifierTerminal("identifier");
            var VarName = Id;
            var ClassName = Id;
            var FunctionName = Id;
            var ArgVarname = Id;

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
            var VariableSet = new NonTerminal("VariableSet", typeof(Node.VariableCall));
            var Primary = new NonTerminal("Primary", typeof(Node.Primary));

            var SimpleExpr = new NonTerminal("SimpleExpr", typeof(Node.Expr));
            var Args = new NonTerminal("Args", typeof(Node.Args));
            var Funcall = new NonTerminal("Funcall", typeof(Node.Funcall));
            var MethodCall = new NonTerminal("MethodCall");
            var Expr = new NonTerminal("Expr", typeof(Node.Expr));
            var Assignment = new NonTerminal("Assignment", typeof(Node.Assignment));
            var AssignmentExpr = new NonTerminal("AssignmentExpr", typeof(Node.Expr));

            var IfStmt = new NonTerminal("IfStmt", typeof(Node.IfStmt));
            var ArgVarnames = new NonTerminal("ArgVarnames", typeof(Node.ArgVarnames));
            var DefineFunction = new NonTerminal("DefineFunction", typeof(Node.DefineFunction));
            var DefineClass = new NonTerminal("DefineClass", typeof(Node.DefineClass));
            var Stmt = new NonTerminal("Stmt", typeof(Node.Stmt));

            var Stmts = new NonTerminal("Stmts", typeof(Node.Stmts));

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
            ArrayItems.Rule = MakeStarRule(ArrayItems, Comma, Stmt);
            Array.Rule = ToTerm("[") + ArrayItems + ToTerm("]");
            AssocVal.Rule = Stmt + ToTerm("=>") + Stmt;
            Assoc.Rule = MakeStarRule(Assoc, Comma, AssocVal) | Empty;
            Hash.Rule = ToTerm("{") + Assoc + ToTerm("}");
            Block.Rule = ToTerm("do") + Stmts + ToTerm("end");
            Lambda.Rule = ToTerm("->") + ToTerm("do") + Stmts + ToTerm("end");
            Quote.Rule = "'" + SimpleExpr;
            QuasiQuote.Rule = "`" + SimpleExpr;
            QuestionQuote.Rule = "?" + SimpleExpr;
            VariableCall.Rule = VarName;
            VariableSet.Rule = VarName;
            Primary.Rule = Numeric | Str | Bool | Nl | Symbol | Array | Hash | Block | Lambda | Quote | QuasiQuote | QuestionQuote | VariableCall;

            Args.Rule = MakeStarRule(Args, Comma, Stmt);
            Funcall.Rule = FunctionName + "(" + Args + ")";
            MethodCall.Rule = SimpleExpr + "." + Funcall;
            SimpleExpr.Rule = "(" + Stmt + ")" | MethodCall | Funcall | Primary;
            var OpExpr = makeExpressions(operandTable, SimpleExpr);
            Assignment.Rule = VariableSet + ToTerm("=") + OpExpr;
            AssignmentExpr.Rule = OpExpr | Assignment;
            RegisterOperators(0, "=");

            Expr.Rule = AssignmentExpr;

            IfStmt.Rule = ToTerm("if(") + Expr + ToTerm(")") + Stmts + ToTerm("end");
            ArgVarnames.Rule = MakeStarRule(ArgVarnames, ToTerm(","), ArgVarname);
            DefineFunction.Rule = ToTerm("def") + FunctionName + "(" + ArgVarnames + ")" + Stmts + ToTerm("end");
            DefineClass.Rule = ToTerm("class")+ ClassName + Term + Stmts + ToTerm("end");
            Stmt.Rule = DefineClass | DefineFunction | IfStmt | Expr;

            Stmts.Rule = MakeStarRule(Stmts, Term, Stmt);

            Root = Stmts;
        }
         
        NonTerminal makeExpressions(List<object[]> table, NonTerminal expr)
        {
            var Table = table.ToArray();
            NonTerminal OpExpr = expr;
            for (int i = 0; i < Table.Length; i++)
            {
                object[] row = Table[i];
                OperandType type = (OperandType)row[0];
                int precedence = (Table.Length - i);
                string[] operands = (string[])row[1];
                OpExpr = makeExpr(type, precedence, operands, OpExpr);
            }
            return OpExpr;
        }


        NonTerminal makeExpr(OperandType type, int precedence, string[] operands, NonTerminal primaryExpr)
        {
            var Expr = new NonTerminal("Expr(" + string.Join(",", operands) + ")", typeof(Node.Expr));
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
