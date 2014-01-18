using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using System.Diagnostics;

namespace LP
{
    // TODO: ハッシュ作成
    // TODO: case文
    // TODO: メソッド定義
    // TODO: class定義、module定義
    class LpParser
    {
        static readonly Parser<char> Term = Parse.Char(';').Or(Parse.Char('\n'));

        // Primary Values
        static readonly Parser<string> Identifier = (from first in Parse.Letter.Or(Parse.Char('_')).Once()
                                                     from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many()
                                                     select new string(first.Concat(rest).ToArray()));

        static readonly Parser<string> Operands = from a in Parse.String("(")
                                                  from v in new string[] { "**", "*", "/", "%", "+", "-", "<<", ">>", "&", "|", ">=", ">", "<=", "<", "<=>", "===", "==", "!=", "=~", "!~", "&&", "||", "and", "or" }.Select(op => Parse.String(op)).Aggregate((op1, op2) => op1.Or(op2)).Text()
                                                  from b in Parse.String(")")
                                                  select v;
        static readonly Parser<string> Fname = Operands.Or(Identifier);

        static readonly Parser<string> Decimal = from a in Parse.Digit.AtLeastOnce().Text()
                                                 from dot in Parse.Char('.').Once().Text()
                                                 from b in Parse.Digit.AtLeastOnce().Text()
                                                 select a + dot + b;

        static readonly Parser<string> Bool = Parse.String("true").Or( Parse.String("false") ).Text().Token();
        static readonly Parser<string> Int = Parse.Digit.AtLeastOnce().Text().Token();
        static readonly Parser<string> Numeric = Decimal.Or(Int).Token();
        static readonly Parser<string> Symbol = from mark in Parse.String(":").Text()
                                                from idf in Identifier
                                                select mark+idf;
        static readonly Parser<string> String = from a in Parse.Char('"')
                                                from s in ( Parse.Char('\\').Once().Concat( Parse.Char('"').Once() )).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                from b in Parse.Char('"')
                                                select '"' + string.Join("", s.ToArray() ) + '"';
        // Complex Data Values
        static readonly Parser<string> SepElm = (from sep in Parse.Char(',').Token()
                                                 from s in Parse.Ref(() => Stmt)
                                                 select s).Or(Parse.Ref(() => Stmt));
        static readonly Parser<string> Array = from a in Parse.String("[").Text().Token()
                                               from elms in SepElm.Many()
                                               from b in Parse.String("]").Text().Token()
                                               select a + string.Join(",", elms) + b;

        static readonly Parser<string> Block = from a in Parse.String("do").Token()
                                               from stmts in Parse.Ref( ()=> Stmts )
                                               from c in Parse.String("end").Token()
                                               select "do " + string.Join("; ", stmts.ToArray() ) + " end";
        static readonly Parser<string> Lambda = from a in Parse.String("^do").Token()
                                                from stmts in Parse.Ref(() => Stmts)
                                                from c in Parse.String("end")
                                                select "^do " + string.Join("; ", stmts.ToArray()) + " end";
        static readonly Parser<string> Function = from a in Parse.String("def").Token()
                                                  from fname in Identifier
                                                  from b in Term
                                                  from stmts in Stmts
                                                  from c in Parse.String("end")
                                                  select "(:" + fname + ").(=)" + "(^do " + 
                                                         string.Join( "; ", stmts.ToArray()) + 
                                                         " end)";

        static readonly Parser<string> Funcall = (from idf in Fname
                                                  from c in Parse.Char('(').Once().Text()
                                                  from args in Args
                                                  from d in Parse.Char(')').Once().Text()
                                                  select idf + c + string.Join(", ", args.ToArray()) + d).Or(
                                                  from idf in Fname
                                                  select idf+"()");

        static readonly Parser<string> Primary = Numeric.Or(Bool).Or(String).Or(Symbol).Or(Array).Or(Lambda).Or(Block);

        // Expressions
        string[][] operandTable = new string[][] {
            new string[]{ "**" },
            new string[]{ "*","/" },
            new string[]{ "+","-", "%" },
            new string[]{ "<<",">>" },
            new string[]{ "&" },
            new string[]{ "|" },
            new string[]{ ">=", ">", "<=", "<" },
            new string[]{ "<=>", "===", "==", "!=", "=~", "!~" },
            new string[]{ "&&" },
            new string[]{ "||" },
            new string[]{ "..", "^..", "..^", "^..^" },
            new string[]{ "and", "or" }
        };

        static readonly Parser<string> ExpVal = (from a in Parse.Char('(').Token()
                                                 from v in Expr
                                                 from b in Parse.Char(')').Token()
                                                 select a + v + b).Or(Primary);
        // ::
        static readonly Parser<string> ExpClasscall = ExpVal;

        // method call
        /*
        static readonly Parser<string> MethodCall = from a in ExpClasscall
                                                    from funcalls in (from dot in Parse.Char('.').Once().Text()
                                                                      from funcall in Funcall
                                                                      select dot + funcall).AtLeastOnce()
                                                    select a + string.Join( "", funcalls.ToArray() );
        */
        static readonly Parser<string> MethodCall = MethodCallStart( Parse.Char('.').Once().Text(), ExpClasscall, Funcall );

        static Parser<string> MethodCallStart(
          Parser<string> op,
          Parser<string> operand,
          Parser<string> operand2)
        {
            Func<string, string, string, string> apply = (opr, a, b) => a +opr+ b;
            return operand.Then(first => MethodCallRest(first, op, operand, operand2, apply ));
        }

        static Parser<string> MethodCallRest(
            string firstOperand,
            Parser<string> op,
            Parser<string> operand,
            Parser<string> operand2,
            Func<string, string, string, string> apply)
        {
            return Parse.Or(op.Then(opvalue =>
                          operand2.Then(operandValue => MethodCallRest(apply(opvalue, firstOperand, operandValue), op, operand, operand2, apply))),
                      Parse.Return(firstOperand));
        }

        // .
        static readonly Parser<string> ExpFuncall = MethodCall.Or(ExpClasscall);
        // []
        static readonly Parser<string> ExpArrayAt = ExpFuncall;
        // ++, --
        static readonly Parser<string> ExpUnary = ExpArrayAt;
        // +(単項)  !  ~
        static readonly Parser<string> ExpUnaryPlus = ExpUnary;
        // not
        static readonly Parser<string> ExpNot = ExpUnaryPlus;
        // **
        static readonly Parser<string> ExpSquare = makeExpr(new string[] { "**" }, ExpNot);
        // -(単項)
        static readonly Parser<string> ExpUnaryMinus = ExpSquare;
        // *, /
        static readonly Parser<string> ExpMul = makeExpr(new string[] { "*", "/", "%" }, ExpUnaryMinus);
        // +,-
        static readonly Parser<string> ExpAdditive = makeExpr(new string[] { "+", "-" }, ExpMul);
        // << >>
        static readonly Parser<string> ExpShift = makeExpr(new string[] { "<<", ">>" }, ExpAdditive);
        // & 
        static readonly Parser<string> ExpAnd = makeExpr(new string[] { "&" }, ExpShift);
        // |
        static readonly Parser<string> ExpInclusiveOr = makeExpr(new string[] { "|" }, ExpAnd);
        // ^ 
        static readonly Parser<string> ExpRelational = ExpInclusiveOr;
        // > >=  < <= 
        static readonly Parser<string> ExpExclusiveOr = makeExpr(new string[] { ">=", ">", "<=", "<" }, ExpRelational);
        // <=> ==  === !=  =~  !~ 
        static readonly Parser<string> ExpEquality = makeExpr(new string[] { "<=>", "===", "==", "!=", "=~", "!~" }, ExpExclusiveOr);
        // &&
        static readonly Parser<string> ExpLogicalAnd = makeExpr(new string[] { "&&" }, ExpEquality);
        // ||
        static readonly Parser<string> ExpLogicalOr = makeExpr(new string[] { "||" }, ExpLogicalAnd);
        // ..
        static readonly Parser<string> ExpRange = makeExpr(new string[] { ".." }, ExpLogicalOr);
        // =(+=, -= ... )
        static readonly Parser<string> ExpAssignment = ExpRange;
        // and or
        static readonly Parser<string> ExpAndOr = makeExpr(new string[] { "and", "or" }, ExpAssignment);
        // =
        static readonly Parser<string> ExpEqual = Parse.ChainOperator(
            Operator("="),
            ExpAndOr,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");

        // 演算子一覧
        static readonly Parser<string> Expr = ExpEqual;

        static readonly Parser<string> Arg = Parse.Ref(() => Stmt);
        static readonly Parser<string[]> Args = from ags in
                                                    (from sep in Parse.Char(',').Token()
                                                     from s in Arg
                                                     select s).Or(Arg).Many()
                                                select ags.ToArray();
        static readonly Parser<string[]> ArgsSet = from a in Parse.Char('(').Once().Text()
                                                   from args in Args
                                                   from b in Parse.Char(')').Once().Text()
                                                   select args.ToArray();

        // Macro Values
        static readonly Parser<string> Quote = from qmark in Parse.String("'").Text()
                                               from idf in Stmt
                                               select idf;

        static readonly Parser<string> QuasiQuote = from qmark in Parse.String("`").Text()
                                                    from idf in Stmt
                                                    select idf;

        // Statements
        static readonly Parser<string> IfStmt1 = from a in Parse.String("if(").Token()
                                                 from expr in Expr
                                                 from b in Parse.String(")").Token()
                                                 from stmts1 in Stmts
                                                 from c in Parse.String("end").Token()
                                                 select "if(" + expr + ",do " + string.Join("; ", stmts1.ToArray()) + " end)";
        static readonly Parser<string> IfStmt2 = from a in Parse.String("if(").Token()
                                                 from expr in Expr
                                                 from b in Parse.String(")").Token()
                                                 from stmts1 in Stmts
                                                 from els in Parse.String("else").Token()
                                                 from stmts2 in Stmts
                                                 from c in Parse.String("end").Token()
                                                 select "if(" + expr + ",do " + string.Join("; ", stmts1.ToArray()) + " end,do " + string.Join("; ", stmts2.ToArray()) + " end)";
        static readonly Parser<string> IfStmt = IfStmt2.Or(IfStmt1);

        static readonly Parser<string> StatCollection = IfStmt;
        static readonly Parser<string> StatList = StatCollection.Or(Expr);

        static readonly Parser<string> Stmt = (from s in StatList
                                               from t in Term
                                               select s).Or(StatList);

        static readonly Parser<string[]> Stmts = from stmts in Stmt.Many()
                                                 select stmts.ToArray();

        static readonly Parser<string> Program = from stmts in Stmts
                                                 select string.Join( "; ", stmts.ToArray() );

        // Primary Values
        static readonly Parser<Object.LpObject> INT = from n in Int
                                                      select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> NUMERIC = from n in Numeric
                                                          select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> BOOL = from b in Bool
                                                       select Object.LpBool.initialize( bool.Parse(b) );

        static readonly Parser<Object.LpObject> STRING = from a in Parse.Char('"')
                                                         from s in Parse.CharExcept('"').Many().Text()
                                                         from b in Parse.Char('"')
                                                         select Object.LpString.initialize(s);

        static readonly Parser<Object.LpObject> SYMBOL = from m in Parse.String(":").Text()
                                                         from s in Identifier
                                                         select Object.LpSymbol.initialize(s);

        static readonly Parser<Object.LpObject> ARRAY = from a in Parse.String("[").Text().Token()
                                                        from elms in SepElm.Many()
                                                        from b in Parse.String("]").Text().Token()
                                                        select elms.Aggregate(Object.LpArray.initialize(), (args, s) => args.funcall("push", PRIMARY.Parse(s)));

        static readonly Parser<Object.LpObject> HASH = from a in Parse.String("{").Text().Token()
                                                       from elms in SepElm.Many()
                                                       from b in Parse.String("}").Text().Token()
                                                       select makeHash(elms.ToArray());

        public static readonly Parser<Object.LpObject> PRIMARY = NUMERIC.Or(BOOL).Or(STRING).Or(SYMBOL).Or(ARRAY);

        static readonly Parser<Object.LpObject> ARGS = from gs in Args
                                                       select gs.ToArray().Aggregate(Object.LpArguments.initialize(), (args, s) => { args.funcall("push", PRIMARY.Parse(s)); return args; });

        static readonly Parser<Object.LpObject> TYPE_ARGS = from brace1 in Parse.Char('(')
                                                            from args in ARGS.Token()
                                                            from brace2 in Parse.Char(')')
                                                            select args;
        static readonly Parser<Object.LpObject> FUNCALL = from obj in Parse.Ref( () => PRIMARY )
                                                          from op in Parse.Char('.')
                                                          from fname in Fname
                                                          from args in TYPE_ARGS
                                                          select obj.funcall(fname, args);

        static readonly Parser<Object.LpObject> BLOCK = from a in Parse.String("do").Token()
                                                        from stmts in Stmts
                                                        from b in Parse.String("end").Token()
                                                        select stmts.ToArray().Aggregate(Object.LpBlock.initialize(), (o, s) => { o.statements.Add(s); return o; });

        static readonly Parser<Object.LpObject> LAMBDA = from a in Parse.String("^do").Token()
                                                         from stmts in Stmts
                                                         from b in Parse.String("end").Token()
                                                         select stmts.ToArray().Aggregate(Object.LpBlock.initialize(), (o, s) => { o.statements.Add(s); return o; });

        static readonly Parser<Object.LpObject> EXPR = FUNCALL.Or(PRIMARY);
        public static readonly Parser<Object.LpObject> STMT = EXPR;

        static readonly Parser<Object.LpObject> PROGRAM = from stmts in Stmts
                                                          select doStmts( stmts.ToArray() );

        static Parser<string> makeExpr(string[] operators, Parser<string> beforeExpr )
        {
            return Parse.ChainOperator(
                operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2)),
                beforeExpr,
                (op, a, b) => a + ".(" + op + ")(" + b + ")");
        }

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

        static Object.LpObject makeHash(string[] os)
        {
            Object.LpObject args = Object.LpHash.initialize();
            return args;
        }

        static Object.LpObject doStmts(string[] stmts)
        {
            Object.LpObject ret=null;
            foreach (var stmt in stmts)
            {
                ret = STMT.Parse(stmt);
            }
            return ret;
        }

        // 単体テスト時にアクセスしやすいように
        static string parseString( Parser<string> psr, string ctx ) {
            return psr.Parse( ctx );
        }

        // 単体テスト時にアクセスしやすいように
        static string[] parseArrString(Parser<string[]> psr, string ctx)
        {
            return psr.Parse(ctx);
        }

        // 単体テスト時にアクセスしやすいように
        static Object.LpObject parseObject(Parser<Object.LpObject> psr, string ctx)
        {
            return psr.Parse(ctx);
        }

        static Object.LpObject parseArrObject(Parser<Object.LpObject> psr, string ctx)
        {
            Console.WriteLine(ctx);
            return psr.Parse(ctx);
        }

        public static Object.LpObject execute(string ctx)
        {
            Parser<string> expander = Program;
            var expanded_code = expander.Parse(ctx);
            Console.WriteLine("Expanded Code:");
            Console.WriteLine(expanded_code);
            var parser = PROGRAM;
            Console.WriteLine("Exec Program:");
            return parser.Parse(expanded_code);
        }
    }
}
