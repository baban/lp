using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP
{
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

        static readonly Parser<string> Int = Parse.Digit.Many().Text().Token();
        static readonly Parser<string> Numeric = Decimal.Or(Int).Token();

        static readonly Parser<string> String = from a in Parse.Char('"')
                                                from s in Parse.LetterOrDigit.Many().Text()
                                                from b in Parse.Char('"')
                                                select "\""+s+"\"";

        static readonly Parser<string> Primary = Numeric;
        static readonly Parser<string> Arg = Numeric;
        static readonly Parser<string> SepArg = (from sep in Parse.Char(',')
                                                 from s in Arg
                                                 select s);
        static readonly Parser<string[]> Args = from ags1 in Arg
                                                from ags in SepArg.Many()
                                                select ags1=="" ?
                                                        new string[] {} :
                                                        new string[] { ags1 }.Concat( ags.ToArray() ).ToArray();

        static readonly Parser<string> Operands = Parse.String("(+)").Or(Parse.String("(-)")).Or(Parse.String("(*)")).Or(Parse.String("(/)")).Text();
        static readonly Parser<string> Fname = Operands.Or(Identifier);
        // +,-
        static readonly Parser<string> ExpAdditive = from a in Numeric.Token()
                                                     from op in (Parse.String("+").Or(Parse.String("-"))).Token().Text()
                                                     from b in Numeric.Token()
                                                     select a + ".(" + op + ")(" + b + ")";
        // *, /
        static readonly Parser<string> ExpMul = from a in Numeric.Token()
                                                from op in (Parse.String("*").Or(Parse.String("/"))).Token().Text()
                                                from b in Numeric.Token()
                                                select a + ".(" + op + ")(" + b + ")";

        // 演算子一覧
        static readonly Parser<string> Expr = ExpAdditive;

        static readonly Parser<string> Stmt = from s in Primary
                                              from t in Parse.Char(';').Or( Parse.Char('\n') )
                                              select s;

        static readonly Parser<string> Quote = from qmark in Parse.String("'").Text()
                                               from idf in Primary
                                               select idf;

        static readonly Parser<string> QuasiQuote = from qmark in Parse.String("`").Text()
                                                    from idf in Primary
                                                    select idf;

        static readonly Parser<Object.LpObject> INT = from n in Int
                                                      select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> STRING = from a in Parse.Char('"')
                                                         from s in Parse.LetterOrDigit.Many().Text()
                                                         from b in Parse.Char('"')
                                                         select Object.LpString.initialize(s);

        static readonly Parser<Object.LpObject> NUMERIC = from n in Numeric
                                                          select Object.LpNumeric.initialize(double.Parse(n));

        public static readonly Parser<Object.LpObject> PRIMARY = NUMERIC;
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
                                                        from tks in Stmt.Token().Many()
                                                        from b in Parse.String("end").Token()
                                                        select makeBlock(tks.ToArray());


        static Object.LpObject makeArgs( string[] os )
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
