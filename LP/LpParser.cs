using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP
{
    // TODO: ハッシュ作成
    // TODO: メソッド呼び出し
    // TODO: if文, case文
    // TODO: メソッド定義
    // TODO: class定義、module定義
    class LpParser
    {
        static readonly Parser<string> Identifier =
                                            from first in Parse.Letter.Or(Parse.Char('_')).Once()
                                            from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many()
                                            select new string(first.Concat(rest).ToArray());

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

        static readonly Parser<string> Primary = Numeric.Or(Bool).Or(String);

        static readonly Parser<string> SepElm = (from sep in Parse.Char(',').Token()
                                                 from s in Primary
                                                 select s).Or(Primary);
        static readonly Parser<string> Array = from a in Parse.String("[").Text().Token()
                                               from elms in SepElm.Many()
                                               from b in Parse.String("]").Text().Token()
                                               select a + string.Join(",", elms) + b;

        static readonly Parser<string> Arg = Primary;
        static readonly Parser<string> SepArg = (from sep in Parse.Char(',').Token()
                                                 from s in Arg
                                                 select s).Or(Arg);
        static readonly Parser<string[]> Args = from ags in SepArg.Many()
                                                select ags.ToArray();

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
            new string[]{ "and", "or" }
        };

        static readonly Parser<string> Operands = from a in Parse.String("(")
                                                  from v in new string[] { "**", "*", "/", "%", "+", "-", "<<", ">>", "&", "|", ">=", ">", "<=", "<", "<=>", "===", "==", "!=", "=~", "!~", "&&", "||", "and", "or" }.Select(op => Parse.String(op)).Aggregate( (op1, op2) => op1.Or(op2) ).Text()
                                                  from b in Parse.String(")")
                                                  select v;
        static readonly Parser<string> Fname = Operands.Or(Identifier);
        static readonly Parser<string> ExpVal = (from a in Parse.Char('(')
                                                 from v in Expr
                                                 from b in Parse.Char(')')
                                                 select a+v+b).Or(Primary);
        // ::
        // .
        // []
        static readonly Parser<string> ExpPostfix = ExpVal;
        // +(単項)  !  ~
        // not
        // **
        static readonly Parser<string> ExpSquare = Parse.ChainOperator(
            Operator("**"),
            ExpPostfix,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // -(単項)
        // *, /
        static readonly Parser<string> ExpMul = Parse.ChainOperator(
            Operator("*").Or(Operator("/")).Or(Operator("%")),
            ExpSquare,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // +,-
        static readonly Parser<string> ExpAdditive = Parse.ChainOperator(
            Operator("+").Or(Operator("-")),
            ExpMul,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // << >>
        static readonly Parser<string> ExpShift = Parse.ChainOperator(
            Operator("<<").Or(Operator(">>")),
            ExpAdditive,
            (op, a, b) => a + ".(" + op + ")(" + b + ")"); 
        // & 
        static readonly Parser<string> ExpAnd = Parse.ChainOperator(
            Operator("&"),
            ExpShift,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // |
        static readonly Parser<string> ExpInclusiveOr = Parse.ChainOperator(
            Operator("|"),
            ExpAnd,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // ^ 
        static readonly Parser<string> ExpRelational = ExpInclusiveOr;
        // > >=  < <= 
        static readonly Parser<string> ExpExclusiveOr = Parse.ChainOperator(
            Operator(">=").Or(Operator(">")).Or(Operator("<=")).Or(Operator("<")),
            ExpRelational,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // <=> ==  === !=  =~  !~ 
        static readonly Parser<string> ExpEquality = Parse.ChainOperator(
            Operator("<=>").Or(Operator("===")).Or(Operator("==")).Or(Operator("!=")).Or(Operator("=~")).Or(Operator("!~")),
            ExpExclusiveOr,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // &&
        static readonly Parser<string> ExpLogicalAnd = Parse.ChainOperator(
            Operator("&&"),
            ExpEquality,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // ||
        static readonly Parser<string> ExpLogicalOr = Parse.ChainOperator(
            Operator("||"),
            ExpLogicalAnd,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // ..  ...
        static readonly Parser<string> ExpRange = Parse.ChainOperator(
            Parse.String("...").Or(Parse.String("..")),
            ExpLogicalOr,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // =(+=, -= ... )
        static readonly Parser<string> ExpAssignment = ExpRange;
        // and or
        static readonly Parser<string> ExpAndOr = Parse.ChainOperator(
            Operator("and").Or(Operator("or")),
            ExpAssignment,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        // =
        /*
        static readonly Parser<string> ExpEqual = Parse.ChainOperator(
            Operator("="),
            ExpAndOr,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");
        */
        static readonly Parser<string> ExpEqual = ExpAndOr;

        // 演算子一覧
        static readonly Parser<string> Expr = ExpEqual;

        static readonly Parser<string> IfExpr1 = from a in Parse.String("if(").Token()
                                                 from expr in Expr
                                                 from b in Parse.String(")").Token()
                                                 from stmts1 in Stmt.Many()
                                                 from c in Parse.String("end").Token()
                                                 select "if(" + expr + ",do " + string.Join(";", stmts1.ToArray()) + " end)";
        static readonly Parser<string> IfExpr2 = from a in Parse.String("if(").Token()
                                                 from expr in Expr
                                                 from b in Parse.String(")").Token()
                                                 from stmts1 in Stmt.Many()
                                                 from els in Parse.String("else").Token()
                                                 from stmts2 in Stmt.Many()
                                                 from c in Parse.String("end").Token()
                                                 select "if(" + expr + ",do " + string.Join(";", stmts1.ToArray()) + " end,do " + string.Join(",", stmts2.ToArray()) + " end)";
        static readonly Parser<string> IfExpr = IfExpr2.Or(IfExpr1);

        static readonly Parser<string> Stmt = (from s in Expr
                                              from t in Parse.Char(';').Or( Parse.Char('\n') )
                                              select s).Or(Expr);

        static readonly Parser<string> Quote = from qmark in Parse.String("'").Text()
                                               from idf in Stmt
                                               select idf;

        static readonly Parser<string> QuasiQuote = from qmark in Parse.String("`").Text()
                                                    from idf in Stmt
                                                    select idf;

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
                                                        select makeArray(elms.ToArray());

        static readonly Parser<Object.LpObject> HASH = from a in Parse.String("{").Text().Token()
                                                       from elms in SepElm.Many()
                                                       from b in Parse.String("}").Text().Token()
                                                       select makeHash(elms.ToArray());

        public static readonly Parser<Object.LpObject> PRIMARY = NUMERIC.Or(BOOL).Or(STRING).Or(SYMBOL).Or(ARRAY);

        static readonly Parser<Object.LpObject> ARG = PRIMARY;
        static readonly Parser<Object.LpObject> ARGS = from gs in Args
                                                       select makeArgs( gs );
         
        static readonly Parser<Object.LpObject> FUNCALL = from obj in NUMERIC
                                                          from dot in Parse.Char('.').Once()
                                                          from fname in Fname
                                                          from brace1 in Parse.Char('(').Once()
                                                          from args in ARGS.Token()
                                                          from brace2 in Parse.Char(')').Once()
                                                          select obj.funcall(fname, args);

        static readonly Parser<Object.LpObject> BLOCK = from a in Parse.String("do").Token()
                                                        from stmts in Stmt.Token().Many()
                                                        from b in Parse.String("end").Token()
                                                        select makeBlock(stmts.ToArray());

        static readonly Parser<Object.LpObject> EXPR = FUNCALL.Or(PRIMARY);
        public static readonly Parser<Object.LpObject> STMT = EXPR;

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

        static Object.LpObject makeHash(string[] os)
        {
            Object.LpObject args = Object.LpHash.initialize();
            return args;
        }

        static Object.LpObject makeArray(string[] os)
        {
            Object.LpObject args = Object.LpArray.initialize();
            foreach (var v in os)
            {
                args.funcall("push", ARG.Parse(v));
            }
            return args;
        }

        static Object.LpObject makeArgs(string[] os)
        {
            Object.LpObject args = Object.LpArguments.initialize();
            foreach (var v in os) {
                args.funcall("push", ARG.Parse(v) );
            }
            return args;
        }

        static Object.LpObject makeBlock(string[] os)
        {
            Object.LpObject o = Object.LpBlock.initialize();
            foreach (var v in os)
            {
                o.statements.Add(v);
            }
            return o;
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
    }
}
