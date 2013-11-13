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

        static readonly Parser<string> Int = Parse.Digit.Many().Token().Text();
        static readonly Parser<string> Numeric = Decimal.Or(Int).Token();
        static readonly Parser<string> Arg = Numeric;
        static readonly Parser<string> SepArg = (from sep in Parse.Char(',').Token()
                                                 from s in Arg
                                                 select s).Or(Arg);
        static readonly Parser<List<string>> Args = from ags in SepArg.Once()
                                                select ags.ToList();

        static readonly Parser<LP.Object.LpObject> NUMERIC = from n in Numeric
                                                             select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> PRIMARY = NUMERIC;
        static readonly Parser<Object.LpObject> ARG = PRIMARY;
        static readonly Parser<Object.LpObject> ARGS = from gs in Args
                                                       select makeArgs( gs );

        static Object.LpObject makeArgs( List<string> os )
        {
            Object.LpObject args = Object.LpArguments.initialize();
            os.ForEach(delegate(string s) { args.funcall("push", ARG.Parse(s)); });
            return args;
        }

        // 単体テスト時にアクセスしやすいように
        static string parseString( Parser<string> psr, string ctx ) {
            return psr.Parse( ctx );
        }

        // 単体テスト時にアクセスしやすいように
        static List<string> parseArrString(Parser<List<string>> psr, string ctx)
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
