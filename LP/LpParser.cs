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
                                            from first in Parse.Letter.Once()
                                            from rest in Parse.LetterOrDigit.Many()
                                            select new string(first.Concat(rest).ToArray());

        static readonly Parser<string> Decimal = from a in Parse.Digit.Many().Text()
                                                 from dot in Parse.Char('.').Once().Text()
                                                 from b in Parse.Digit.Many().Text()
                                                 select a + dot + b;

        static readonly Parser<string> Int = Parse.Digit.Many().Text().Token();
        static readonly Parser<string> Numeric = Decimal.Or(Int).Token();
        static readonly Parser<string> Arg = Numeric;
        static readonly Parser<string> SepArg = (from sep in Parse.Char(',')
                                                 from s in Arg
                                                 select s);
        static readonly Parser<string[]> Args = from ags1 in Arg
                                                from ags in SepArg.Many()
                                                select ags1=="" ?
                                                        new string[] {} :
                                                        new string[] { ags1 }.Concat( ags.ToArray() ).ToArray();

        static readonly Parser<LP.Object.LpObject> NUMERIC = from n in Numeric
                                                             select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> PRIMARY = NUMERIC;
        static readonly Parser<Object.LpObject> ARG = PRIMARY;
        static readonly Parser<Object.LpObject> ARGS = from gs in Args
                                                       select makeArgs( gs );

        static Object.LpObject makeArgs( string[] os )
        {
            Object.LpObject args = Object.LpArguments.initialize();
            foreach (var v in os) {
                args.funcall("push", ARG.Parse(v) );
            }
            return args;
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
