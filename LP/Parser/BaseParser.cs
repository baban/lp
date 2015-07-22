using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Parser
{
    class BaseParser
    {
        static readonly Parser<string> OperandMarks = new string[] { "**", "*", "/", "%", "+", "-", "<<", ">>", "&", "|", ">=", ">", "<=", "<", "<=>", "===", "==", "!=", "=~", "!~", "&&", "||", "and", "or", "=" }.Select(op => Parse.String(op)).Aggregate((op1, op2) => op1.Or(op2)).Text();

        // 基本文字一覧
        protected static readonly Parser<string> Term = Parse.Regex("^[;\n]");

        protected static readonly Parser<string> ReservedNames = new string[] { "true", "false", "do", "end", "if", "else", "def", "case", "when" }.Select((s) => Parse.String(s).Text()).Aggregate((seed, nxt) => seed.Or(nxt)).Token();

        // Primary Values
        protected static readonly Parser<string> Identifier = Parse.Except(Parse.Regex("[a-zA-Z_][a-zA-Z0-9_]*"), ReservedNames);

        protected static readonly Parser<string> Fname = (from a in Parse.String("(").Text()
                                                from v in OperandMarks.Or(Identifier)
                                                from b in Parse.String(")").Text()
                                                select a + v + b).Or(Identifier);

        protected static readonly Parser<string> Varname = Identifier;

        // Comment
        protected static readonly Parser<string> InlineComment = Parse.Regex("//.*?\n").Return("").Named("lnline comment");
        protected static readonly Parser<string> BlockComment = Parse.Regex(@"/\*.*?\*/").Return("").Named("block comment");

        protected static readonly Parser<string> Comment = InlineComment.Or(BlockComment);

        protected static readonly Parser<string> Nl = Parse.Regex("nl").Named("Nl");
        protected static readonly Parser<string> Bool = Parse.Regex("true|false").Named("boolean");
        protected static readonly Parser<string> Decimal = Parse.Regex(@"\d+\.\d+");
        protected static readonly Parser<string> Int = Parse.Regex(@"\d+");
        protected static readonly Parser<string> BinaryInt = from a in Parse.String("0b")
                                                   from b in Parse.Regex(@"[01]+")
                                                   select Convert.ToInt32(b, 2).ToString();
        protected static readonly Parser<string> OctetInt = from a in Parse.String("0o")
                                                  from b in Parse.Regex(@"[0-7]+")
                                                  select Convert.ToInt32(b, 8).ToString();
        protected static readonly Parser<string> DigitInt = from a in Parse.String("0x")
                                                  from b in Parse.Regex(@"[0-9a-z]+")
                                                  select Convert.ToInt32(b, 16).ToString();
        protected static readonly Parser<string> Numeric = BinaryInt.Or(OctetInt).Or(DigitInt).Or(Decimal).Or(Int).Named("numeric");

        protected static readonly Parser<string> GlobalVarname = from h in Parse.String("$").Text()
                                                       from name in Varname
                                                       select string.Format("{0}{1}", h, name);

        protected static readonly Parser<string> InstanceVarname = from h in Parse.String("@").Text()
                                                         from name in Varname
                                                         select string.Format("{0}{1}", h, name);

        protected static readonly Parser<string> ClassVarname = from h in Parse.String("@@").Text()
                                                      from name in Varname
                                                      select string.Format("{0}{1}", h, name);
        protected static readonly Parser<string> TotalVarname = Varname.Or(GlobalVarname).Or(InstanceVarname).Token();


        public enum NodeType
        {
            NL, INT, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, GLOBAL_VARIABLE_CALL, INSTANCE_VARIABLE_CALL, VARIABLE_CALL,
            LAMBDA, BLOCK, PRIMARY,
            ARGS, FUNCALL, EXPR, EXP_VAL, FUNCTION_CALL, STMT, STMTS, PROGRAM, HASH, QUOTE, QUASI_QUOTE, QUESTION_QUOTE
        };
        protected static readonly Parser<object[]> NL = from s in Nl
                                              select new object[] { NodeType.NL, s };
        protected static readonly Parser<object[]> INT = from n in Int
                                               select new object[] { NodeType.INT, n };
        protected static readonly Parser<object[]> NUMERIC = from n in Numeric
                                                   select new object[] { NodeType.NUMERIC, n };
        protected static readonly Parser<object[]> BOOL = from b in Bool
                                                select new object[] { NodeType.BOOL, b };
        protected static readonly Parser<object[]> SYMBOL = from m in Parse.String(":").Text()
                                                  from s in TotalVarname
                                                  select new object[] { NodeType.SYMBOL, s };
        //public static readonly Parser<object[]> PRIMARY = new Parser<object[]>[] { NL, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, HASH, LAMBDA, BLOCK, QUOTE, QUASI_QUOTE, QUESTION_QUOTE }.Aggregate((seed, nxt) => seed.Or(nxt));
        protected static List<Parser<object[]>> PRIMARY_PARSERS = new List<Parser<object[]>>() { NL, NUMERIC, BOOL, SYMBOL };
        public static Parser<object[]> PRIMARY = PRIMARY_PARSERS.Aggregate((seed, nxt) => seed.Or(nxt));
    }
}
