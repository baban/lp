using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Parser
{
    class StringParser 
    {
        //static readonly Parser<string> Stmt = MainPerser.Stmt;

        static readonly List<char[]> EscapeCharacters = new List<char[]> {
           new char[]{ '\\' , '\\' },
           new char[]{ '0', '\0' }, // null
           new char[]{ 'a', '\a' }, // beep
           new char[]{ 'b', '\b' }, // back space
           new char[]{ 'f', '\f' }, // form feed
           new char[]{ 'n', '\n' }, // line change
           new char[]{ 'r', '\r' }, // carigde return
           new char[]{ 't', '\t' }, // tab
           new char[]{ 'v', '\v' }, // vartical return
        };
        static readonly Parser<string> AsciiCharacter = from a in Parse.String(@"\x")
                                                        from b in Parse.Regex(@"[0-9a-fA-F]{2}")
                                                        select ((char)Convert.ToInt32(b, 16)).ToString();
        static readonly Parser<string> UnicodeCharacter = from a in Parse.String(@"\u")
                                                          from b in Parse.Regex(@"[0-9a-fA-F]{4}")
                                                          select ((char)Convert.ToInt32(b, 16)).ToString();
        static readonly Parser<string> UnicodeCharacter2 = from a in Parse.String(@"\u{")
                                                           from b in Parse.Regex(@"[0-9a-fA-F]{4}")
                                                           from c in Parse.Regex(@"}")
                                                           select ((char)Convert.ToInt32(b, 16)).ToString();
        static readonly Parser<string> CodeEscapes = UnicodeCharacter.Or(UnicodeCharacter2).Or(AsciiCharacter);
        static readonly Parser<string> EscapeSequence = CodeEscapes.Or(EscapeCharacters.Select((pair) => makeEscapePerser(pair[0], pair[1])).Aggregate((a, b) => a.Or(b)));
        /*
        static readonly Parser<string> StringStmt = from a in Parse.String("#{")
                                                    from stmt in Stmt
                                                    from b in Parse.String("}")
                                                    select "(" + stmt + ").to_s";

        static readonly Parser<string> DoubleStringContent = from s in (Parse.Char('\\').Once().Concat(Parse.Char('"').Once())).Or(Parse.CharExcept('"').Once()).Text().Except(StringStmt).Many()
                                                             select '"' + string.Join("", s.ToArray()) + '"';
        static readonly Parser<string> DoubleQuoteString = (from a in Parse.Char('"')
                                                            from s in DoubleStringContent.Or(StringStmt).Many()
                                                            from b in Parse.Char('"')
                                                            select "(" + string.Join(").(+)(", s.ToArray()) + ")").Named("double quoted string");
        static readonly Parser<string> SingleStringContent = from s in (Parse.Char('\\').Once().Concat(Parse.Char('\'').Once())).Or(Parse.CharExcept('\'').Once()).Text().Except(StringStmt).Many()
                                                             select '"' + string.Join("", s.ToArray()) + '"';
        static readonly Parser<string> SingleQuoteString = (from a in Parse.String("'")
                                                            from s in SingleStringContent.Or(StringStmt).Many()
                                                            from b in Parse.String("'")
                                                            select "(" + string.Join(").(+)(", s.ToArray()) + ")").Named("single quoted string");
         */
        static readonly Parser<string> SingleStringContent = from s in (Parse.Char('\\').Once().Concat(Parse.Char('\'').Once())).Or(Parse.CharExcept('\'').Once()).Text().Many()
                                                             select '"' + string.Join("", s.ToArray()) + '"';
        static readonly Parser<string> SingleQuoteString = (from a in Parse.String("'")
                                                            from s in SingleStringContent.Many()
                                                            from b in Parse.String("'")
                                                            select "(" + string.Join(").(+)(", s.ToArray()) + ")").Named("single quoted string");
        //public static readonly Parser<string> String = DoubleQuoteString.Or(SingleQuoteString).Named("string");
        static readonly Parser<string> DoubleStringContent = from s in (Parse.Char('\\').Once().Concat(Parse.Char('"').Once())).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                             select '"' + string.Join("", s.ToArray()) + '"';
        static readonly Parser<string> DoubleQuoteString = (from a in Parse.Char('"')
                                                            from s in DoubleStringContent.Many()
                                                            from b in Parse.Char('"')
                                                            select "(" + string.Join(").(+)(", s.ToArray()) + ")").Named("double quoted string");
        public static readonly Parser<string> String = DoubleQuoteString.Or(SingleQuoteString).Named("string");
        public static readonly Parser<string> EnableString = from s in EscapeSequence.Or(makeEscapePerser('"', '\"')).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                                               select string.Join("",s);

        static Parser<string> makeEscapePerser(char hint, char ret)
        {
            return from a in Parse.Char('\\').Once().Concat(Parse.Char(hint).Once())
                   select ret.ToString();
        }

    }
}
