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
        // TODO: Arrayの書式不正[,1,2,3]とかかける、を直しておく

        static readonly Parser<string> OperandMarks = new string[] { "**", "*", "/", "%", "+", "-", "<<", ">>", "&", "|", ">=", ">", "<=", "<", "<=>", "===", "==", "!=", "=~", "!~", "&&", "||", "and", "or", "=" }.Select(op => Parse.String(op)).Aggregate((op1, op2) => op1.Or(op2)).Text();
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

        static readonly Parser<string> String = Parser.StringParser.String;

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

        static readonly Parser<string> ArgVarname = Varname;
        static readonly Parser<string> AstArg = from ast in Parse.Char('*')
                                                                        from id in Varname
                                                                        select ast + id;
        static readonly Parser<string> AmpArg = from amp in Parse.Char('&')
                                                                          from id in Varname
                                                                          select amp + id;

        public enum NodeType
        {
            NL, INT, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, GLOBAL_VARIABLE_CALL, INSTANCE_VARIABLE_CALL, VARIABLE_CALL,
            LAMBDA, BLOCK, PRIMARY,
            ARGS, METHODS_CALL, EXPR, EXP_VAL, FUNCALL, STMT, STMTS, PROGRAM, HASH, QUOTE, QUASI_QUOTE, QUESTION_QUOTE
        };

        protected static readonly Parser<object[]> ARG_VARNAMES = from args in Parse.DelimitedBy(ArgVarname, Parse.String(",").Token()).Token()
                                                        select args.ToArray();

        static readonly Parser<object[]> FENCE_ARGS = ARG_VARNAMES.Contained(Parse.String("|").Token(), Parse.String("|").Token()).Token();
        static readonly Parser<object[]> BLACKET_ARGS = ARG_VARNAMES.Contained(Parse.String("("), Parse.String(")")).Token();

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
        protected static readonly Parser<object[]> STRING = from a in Parse.Char('"')
                                                                              from s in EscapeSequence.Or(makeEscapePerser('"', '\"')).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                                              from b in Parse.Char('"')
                                                                              select new object[] { NodeType.STRING, string.Join("", s.ToArray()) };

        protected static readonly Parser<object[]> ARRAY = from elms in (
                                                                                from stmts in Parse.Ref(() => STMT).DelimitedBy(Parse.String(",").Token())
                                                                                from t in Parse.String(",").Token().Optional()
                                                                                select stmts).Contained(Parse.String("[").Text().Token(), Parse.String("]").Text().Token())
                                                                            select new object[] { NodeType.ARRAY, elms.ToList() };
        static readonly Parser<object[]> ASSOC_VAL = from k in Parse.Ref(() => STMT)
                                                                                     from sps in Parse.Char(':').Token()
                                                                                     from s in Parse.Ref(() => STMT)
                                                                                     select new object[] { k, s };
        protected static readonly Parser<object[]> ASSOC = (from sep in Parse.Char(',').Token()
                                                                              from kv in ASSOC_VAL
                                                                              select kv).Or(ASSOC_VAL);
        protected static readonly Parser<object[]> HASH = from pairs in (from ascs in ASSOC_VAL.DelimitedBy(Parse.String(",").Token())
                                                                                                               from t in Parse.String(",").Token().Optional()
                                                                                                               select ascs).Contained(Parse.String("{").Text().Token(), Parse.String("}").Text().Token())
                                                                           select new object[] { NodeType.HASH, pairs.ToArray() };

        static readonly Parser<object[]> BLOCK_START1 = from a in Parse.String("do").Token()
                                                                                           select new object[] { new string[] { }, false };
        static readonly Parser<object[]> BLOCK_START2 = from _do in Parse.String("do").Token()
                                                                                           from args in FENCE_ARGS
                                                                                           select new object[] { args, true };
        static readonly Parser<object[]> BLOCK_START = BLOCK_START2.Or(BLOCK_START1);
        static readonly Parser<object[]> BLOCK_STMT = from argset in BLOCK_START
                                                                                        from stmts in STMTS
                                                                                        from b in Parse.String("end").Token()
                                                                                        select new object[] { (string[])argset[0], (bool)argset[1], stmts };
        protected static readonly Parser<object[]> BLOCK = from blk in BLOCK_STMT.Token()
                                                                             select new object[] { NodeType.BLOCK, blk };
        protected static readonly Parser<object[]> LAMBDA = from head in Parse.String("->").Token()
                                                                               from blk in BLOCK_STMT.Token()
                                                                               select new object[] { NodeType.LAMBDA, blk };

        // Macro Values
        static readonly Parser<object[]> QUOTE = from qmark in Parse.String("'").Text()
                                                                             from expr in EXPR
                                                                             select new object[]{ NodeType.QUOTE, expr };
        static readonly Parser<object[]> QUASI_QUOTE = from qmark in Parse.String("`").Text()
                                                                                    from expr in EXPR
                                                                                    select new object[] { NodeType.QUASI_QUOTE, expr };
        static readonly Parser<object[]> QUESTION_QUOTE = from qmark in Parse.String("?").Text()
                                                                                        from expr in EXPR
                                                                                        select new object[] { NodeType.QUESTION_QUOTE, expr };

        protected static readonly Parser<object[]> EXP_VAL = Parse.Ref(() => STMT).Contained(Parse.Char('('), Parse.Char(')'));
        static readonly Parser<object[]> ZERO_ARGS = from args in Parse.WhiteSpace.Many().DelimitedBy(Parse.String(",").Token())
                                                                                     select new object[]{};
        static readonly Parser<object[]> ARGS = from args in Parse.Ref(() => STMT).DelimitedBy(Parse.String(",").Token())
                                                                          select args.ToArray();
        static readonly Parser<object[]> ARGS_CALL = ARGS.Or(ZERO_ARGS).Contained(Parse.String("("), Parse.String(")"));
        protected static readonly Parser<object[]> METHOD_CALL = from fname in Fname
                                                                                            from args in ARGS_CALL
                                                                                            from blk in BLOCK.Token().Optional()
                                                                                            select new object[] { fname, args, blk.GetOrDefault() };

        protected static readonly Parser<object[]> FUNCALL = from fvals in METHOD_CALL
                                                                                                 select new object[] { NodeType.FUNCALL, fvals };

        static readonly Parser<object[]> VARIABLE_CALL = from name in Varname
                                                                                            select new object[]{ NodeType.VARIABLE_CALL, name };
        static readonly Parser<object[]> GLOBAL_VARIABLE_CALL = from name in GlobalVarname
                                                                                                           select new object[] { NodeType.GLOBAL_VARIABLE_CALL, name };
        static readonly Parser<object[]> INSTANCE_VARIABLE_CALL = from name in InstanceVarname
                                                                                                               select new object[] { NodeType.INSTANCE_VARIABLE_CALL, name };

        protected static readonly Parser<object[]> VARCALL = GLOBAL_VARIABLE_CALL.Or(INSTANCE_VARIABLE_CALL).Or(VARIABLE_CALL);

        protected static List<Parser<object[]>> PRIMARY_PARSERS = new List<Parser<object[]>>() { NL, NUMERIC, BOOL, SYMBOL, STRING, ARRAY, HASH, BLOCK, LAMBDA, QUOTE, QUASI_QUOTE, QUESTION_QUOTE };
        protected static Parser<object[]> PRIMARY = PRIMARY_PARSERS.Aggregate((seed, nxt) => seed.Or(nxt)).Token();

        protected static List<Parser<object[]>> EXPR_PARSERS = new List<Parser<object[]>>() { EXP_VAL, PRIMARY, FUNCALL, VARCALL };
        protected static Parser<object[]> EXPR = makeExprParser();
        protected static Parser<object[]> METHODS_CALL = makeMethodsCallParser();

        protected static List<Parser<object[]>> STMT_PARSERS = new List<Parser<object[]>>() { METHODS_CALL, EXPR };
        protected static Parser<object[]> STMT = makeStmtParser();
        protected static Parser<object[]> STMTS = makeStmtsParser();
        public static Parser<object[]> PROGRAM = makeProgramParser();

        protected static Parser<object[]> makeMethodsCallParser()
        {
            return OperandsChainCallStart(Parse.String("."), EXPR, METHOD_CALL, (dot, op1, op2) =>
            {
                return new object[] {
                NodeType.METHODS_CALL,
                new object[]{
                    (string)op2[0],
                    op1,
                    (object[])op2[1],
                    (object[])op2[2]
                }
            };
            });
        }

        protected static Parser<object[]> makePrimaryParser()
        {
            return PRIMARY_PARSERS.Aggregate((seed, nxt) => seed.Or(nxt)).Token();
        }

        protected static Parser<object[]> makeExprParser()
        {
            return EXPR_PARSERS.Aggregate((seed, nxt) => seed.Or(nxt)).Token();
        }

        protected static Parser<object[]> makeStmtParser()
        {
            return STMT_PARSERS.Aggregate((seed, nxt) => seed.Or(nxt)).Token();
        }

        protected static Parser<object[]> makeStmtsParser()
        {
            return from stmts in STMT.DelimitedBy(Term)
                       from t in Term.Optional().Token()
                       select new object[] { NodeType.STMTS, stmts.ToList() };
        }

        protected static Parser<object[]> makeProgramParser() {
            return STMTS;
        }

        static Parser<string> makeEscapePerser(char hint, char ret)
        {
            return from a in Parse.Char('\\').Once().Concat(Parse.Char(hint).Once())
                   select ret.ToString();
        }

        protected static Parser<T> OperandsChainCallStart<T, T2, TOp>(
          Parser<TOp> op,
          Parser<T> operand,
          Parser<T2> operand2,
          Func<TOp, T, T2, T> apply)
        {
            return operand.Then(first => OperandsChainCallRest(first, op, operand, operand2, apply));
        }

        static Parser<T> OperandsChainCallRest<T, T2, TOp>(
            T firstOperand,
            Parser<TOp> op,
            Parser<T> operand,
            Parser<T2> operand2,
            Func<TOp, T, T2, T> apply)
        {
            return Parse.Or(op.Then(opvalue =>
                          operand2.Then(operandValue => OperandsChainCallRest(apply(opvalue, firstOperand, operandValue), op, operand, operand2, apply))),
                      Parse.Return(firstOperand));
        }

        public static Ast.LpAstNode toNode(object[] node)
        {
            switch ((NodeType)node[0])
            {
                case NodeType.NL:
                    return Ast.LpAstLeaf.toNode((string)node[1], "NL");
                case NodeType.NUMERIC:
                    return Ast.LpAstLeaf.toNode((string)node[1], "NUMERIC");
                case NodeType.STRING:
                    return Ast.LpAstLeaf.toNode((string)node[1], "STRING");
                case NodeType.BOOL:
                    return Ast.LpAstLeaf.toNode((string)node[1], "BOOL");
                case NodeType.SYMBOL:
                    return Ast.LpAstLeaf.toNode((string)node[1], "SYMBOL");
                case NodeType.VARIABLE_CALL:
                    return Ast.LpAstLeaf.toNode((string)node[1], "VARIABLE_CALL");
                case NodeType.GLOBAL_VARIABLE_CALL:
                    return Ast.LpAstLeaf.toNode((string)node[1], "GLOBAL_VARIABLE_CALL");
                case NodeType.INSTANCE_VARIABLE_CALL:
                    return Ast.LpAstLeaf.toNode((string)node[1], "INSTANCE_VARIABLE_CALL");
                case NodeType.QUOTE:
                    return null; // Ast.LpAstQuote.toNode((object[])node[1]);
                case NodeType.QUASI_QUOTE:
                    return null;//Ast.LpAstQuasiQuote.toNode((object[])node[1]);
                case NodeType.QUESTION_QUOTE:
                    return null; ;//Ast.LpAstQuestionQuote.toNode((object[])node[1]);
                case NodeType.FUNCALL:
                    return Ast.LpAstFuncall.toNode((object[])node[1]);
                case NodeType.LAMBDA:
                    return Ast.LpAstLambda.toNode((object[])node[1]);
                case NodeType.BLOCK:
                    return Ast.LpAstBlock.toNode((object[])node[1]);
                case NodeType.METHODS_CALL:
                    return Ast.LpAstMethodCall.toNode((object[])node[1]);
                case NodeType.EXPR:
                    return toNode((object[])node[1]);
                case NodeType.STMTS:
                    return Ast.LpAstStmts.toNode((List<object[]>)node[1]);
                case NodeType.STMT:
                    return toNode( (object[])node[1] );
                case NodeType.ARRAY:
                    return Ast.LpAstArray.toNode((List<object[]>)node[1]);
                case NodeType.HASH:
                    return null; // Ast.LpAstHash.toNode((object[])node[1]);
                default:
                    return null;
            }
        }

        public static Ast.LpAstNode createNode(string ctx)
        {
            Parser<string> Nl = Parse.Regex("nl").Named("Nl");
            string filename = @"C:\Users\ba-n\test.txt";
            System.IO.StreamReader sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8"));
            //var nl = Sprache.ParserExtensions.Parse(Nl, "nl");
            var ipt = new Sprache.Input("nl");
            //var nl = Sprache.ParserExtensions.Parse(Nl, ipt);
            Parser<Sprache.Input> identifier =
                from trailing in Parse.Regex("nl")
                select new Sprache.Input("nl");

            Console.WriteLine("----------------------------execute----------------------------");
            //Console.WriteLine(nl);

            //Console.WriteLine(ctx);
            var pobj = Sprache.ParserExtensions.Parse(PROGRAM, ctx);
            var node = toNode(pobj);
            return node;
        }
    }
}
