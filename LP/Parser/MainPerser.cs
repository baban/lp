using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Parser
{
    class MainPerser : BaseParser
    {
        /*
        // Block,Lambda
        static readonly Parser<string[]> BlaketArgs = Parse.Ref(() => ArgList).Contained(Parse.Char('('), Parse.Char(')'));
        static readonly Parser<string[]> FenceArgs = Parse.Ref(() => ArgList).Contained(Parse.Char('|'), Parse.Char('|'));

        static readonly Parser<string> BlockStart1 = from _do in Parse.String("do").Text()
                                                     from ws in Parse.WhiteSpace.Many()
                                                     select _do;
        static readonly Parser<string> DefMacro = from a in Parse.String("mac").Token()
                                                  from fname in Fname
                                                  from args in ArgDecl
                                                  from b in Term
                                                  from stmts in Stmts
                                                  from c in Parse.String("end")
                                                  select string.Format(
                                                            "Macro.new() do |{1}| {2} end.bind(:{0})",
                                                            fname,
                                                            string.Join(", ", args.ToArray()),
                                                            string.Join("; ", stmts.ToArray()));

        static readonly Parser<string> DefModule = from a in Parse.String("module").Token().Text()
                                                   from cname in Fname
                                                   from b in Term
                                                   from stmts in Stmt.Many().Except(Parse.String("end"))
                                                   from c in Parse.String("end").Token()
                                                   select string.Format("Module.new(:{1}) do {0} end",
                                                      string.Join("; ", stmts), cname);


        static readonly Parser<string> Funcall0 = from idf in Fname
                                                  select idf + "()";
        static readonly Parser<string> Varcall = Varname.Or(GlobalVarname).Or(InstanceVarname).Or(ClassVarname).Token();

        static readonly Parser<string> Primary = new Parser<string>[] { Nl, Numeric, Bool, String, Symbol, Array, Hash, Lambda, Block, Comment, Funcall, Varcall, Quote, QuasiQuote, QuestionQuote }.Aggregate((seed, nxt) => seed.Or(nxt)).Token();

        // arguments
        static readonly Parser<string> Arg = Parse.Ref(() => Stmt);
        static readonly Parser<string[]> ZeroArgs = from sps in Parse.WhiteSpace.Many()
                                                    select new string[] { };
        static readonly Parser<string[]> SimpleArgs = from args in Parse.DelimitedBy(Arg, Parse.Char(','))
                                                      select args.ToArray();
        static readonly Parser<string[]> WithHashArgs = from args in SimpleArgs
                                                        from hash in Assocs
                                                        select args.Concat(new string[] { "{" + string.Join(",", hash) + "}" }).ToArray();
        static readonly Parser<string[]> HashArgs = from hash in Parse.Ref(() => Assocs)
                                                    select new string[] { "{" + string.Join(",", hash) + "}" };
        static readonly Parser<string[]> Args = from ags in new Parser<string[]>[] { SimpleArgs, ZeroArgs }.Aggregate((seed, nxt) => seed.Or(nxt))
                                                select ags.ToArray();
        static readonly Parser<string[]> ArgsCall = from a in Parse.Char('(')
                                                    from args in Args
                                                    from b in Parse.Char(')')
                                                    select args.ToArray();
        
        static readonly Parser<string> StatList = DefMacro.Or(DefClass).Or(DefModule).Or(Function).Or(IfStmt).Or(CaseStmt).Or(Expr);

        static object[] makeHash(string[][] pairs)
        {
            return new object[]{
                NodeType.HASH,
                pairs.Select((pair) => new object[] { STMT.Parse(pair[0]), STMT.Parse(pair[1]) }).ToArray()
            };
        }

        // 単体テスト時にアクセスしやすいように
        static string parseString(Parser<string> psr, string ctx)
        {
            return psr.Parse(ctx);
        }

        // 単体テスト時にアクセスしやすいように
        static string[] parseArrString(Parser<string[]> psr, string ctx)
        {
            return psr.Parse(ctx);
        }

        static Object.LpObject[] parseArgsObject(string ctx)
        {
            return Args.Parse(ctx).ToList().Select((arg) =>
            {
                var node = STMT.Parse(arg);
                return toNode((object[])node).Evaluate();
            }).ToArray();
        }

        static Parser<string> makeEscapePerser(char hint, char ret)
        {
            return from a in Parse.Char('\\').Once().Concat(Parse.Char(hint).Once())
                   select ret.ToString();
        }

        static void benchmarkString(Parser<string> psr, string ctx, int max = 1000)
        {
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < max; i++)
            {
                psr.Parse(ctx);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.WriteLine("benckmark:end");
        }

        static void benchmarkStrings(Parser<string[]> psr, string ctx, int max = 1000)
        {
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < max; i++)
            {
                psr.Parse(ctx);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.WriteLine("benckmark:end");
        }

        static void benchmarkObject(Parser<Object.LpObject> psr, string ctx, int max = 1000)
        {
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < max; i++)
            {
                psr.Parse(ctx);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalSeconds);
            Console.WriteLine("benckmark:end");
        }
        */

        enum OperandType { CHARIN_OPERATOR, OPERAND, LEFT_UNARY, RIGHT_UNARY };
        // Expressions
        static readonly List<object[]> operandTable = new List<object[]> {
            new object[]{ OperandType.CHARIN_OPERATOR, new string[]{ "++", "--" } },
            new object[]{ OperandType.LEFT_UNARY,             new string[]{ "+", "!", "~" } },
            new object[]{ OperandType.RIGHT_UNARY,          new string[]{ "not" } },
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

        static readonly Parser<object[]> SIMPLE_EXP = EXP_VAL.Or(FUNCALL).Or(VARCALL).Or(PRIMARY);

        // ::
        /*
        static readonly Parser<object[]> CLASS_CALL = OperandsChainCallStart(Parse.String("::"), SIMPLE_EXP, METHOD_CALL, (dot, op1, op2) =>
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
        */
        static readonly Parser<object[]> EXP_CLASS_CALL = SIMPLE_EXP;

        // .
        static readonly Parser<object[]> EXP_METHOD_CALL = OperandsChainCallStart(Parse.String("."), EXP_CLASS_CALL, METHOD_CALL, (dot, op1, op2) =>
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

        // []
        static readonly Parser<object[]> ARRAY_AT = from expr in EXP_METHOD_CALL
                                                                                   from arg in STMT.Contained(Parse.Char('['), Parse.Char(']'))
                                                                                   select new object[] { 
                                                                                                  NodeType.METHODS_CALL,
                                                                                                  new object[] { "([])", expr, new object[]{ arg }, null }
                                                                                              };
        static readonly Parser<object[]> EXP_ARRAY_AT = (ARRAY_AT).Or(EXP_METHOD_CALL);
        // ２項演算子一覧
        static readonly Parser<object[]> CHAIN_EXPRS = makeExpressions(operandTable, EXP_ARRAY_AT);
        // TODO: 代入演算子作成
        // =(+=, -= ... )
        static readonly Parser<object[]> EXP_ASSIGNMENT = CHAIN_EXPRS;
        protected static readonly Parser<object[]> EXP_EQUAL_RAW = from varname in TotalVarname
                                                                                                     from op in Parse.String("=").Text().Token()
                                                                                                     from v in EXP_ASSIGNMENT
                                                                                                     select new object[] { 
                                                                                                         NodeType.METHODS_CALL,
                                                                                                         new object[] {
                                                                                                             (string)op,
                                                                                                             new object[]{ NodeType.SYMBOL, varname },
                                                                                                             new object[]{ v },
                                                                                                             null
                                                                                                         }
                                                                                                     };
        protected static readonly Parser<object[]> EXP_EQUAL = EXP_EQUAL_RAW.Or(CHAIN_EXPRS);

        static readonly Parser<object[]> DEFINE_CLASS = makeClassStmtParser();
        static readonly Parser<object[]> DEFINE_FUNCTION = makeDefineFunctionParser();
        static readonly Parser<object[]> IF_STMT = makeIfStmtParser();
        static readonly Parser<object[]> CASE_STMT = makeCaseStmtParser();

        static Parser<object[]> makeClassStmtParser()
        {
            return from a in Parse.String("class").Token().Text()
                   from cname in Fname
                   from stmts in STMTS
                   from c in Parse.String("end").Token()
                   select  new object[]{
                       NodeType.METHODS_CALL,
                       new object[] {
                           "new",
                           new object[]{
                                NodeType.VARIABLE_CALL,
                                "Class",
                            },
                            new object[]{
                                new object[]{
                                    NodeType.SYMBOL,
                                    cname,
                                }
                            },
                            new object[]{
                                NodeType.BLOCK,
                                new object[]{ new string[]{}, true, stmts }
                            }
                       }
                   };
            /*
        static readonly Parser<string> DefClass = from a in Parse.String("class").Token().Text()
                                                  from cname in Fname
                                                  from b in Term
                                                  from stmts in Stmt.Many().Except(Parse.String("end"))
                                                  from c in Parse.String("end").Token()
                                                  select string.Format("Class.new(:{1}) do {0} end",
                                                                    string.Join("; ", stmts), cname);
            */
        }

        static Parser<object[]> makeDefineFunctionParser()
        {
            return from a in Parse.String("def").Token()
                       from fname in Fname
                       from args in ARG_VARNAMES.Contained(Parse.String("("), Parse.String(")"))
                       from stmts in STMTS
                       from c in Parse.String("end")
                       select new object[]{
                           NodeType.METHODS_CALL,
                           new object[] {
                               "bind",
                               new object[]{
                                   NodeType.LAMBDA,
                                   new object[] { args, true, stmts }
                               },
                               new object[]{ new object[]{ NodeType.SYMBOL, fname } },
                               null
                           },
                            null
                       };
        }

        // if Statements
        static Parser<object[]> makeIfStmtParser()
        {
            Parser<object[]> IF_EXPR = Parse.Ref(() => STMT.Contained(Parse.Char('('), Parse.Char(')')).Or(
                                                                                   from stmt in STMT
                                                                                   from b in Term
                                                                                   select stmt));
            Parser<object[]> IF_START = (from _if in Parse.String("if")
                                                                                       from expr in IF_EXPR
                                                                                       select expr).Token();
            Parser<object[]> ELSE_STMTS = 
                                                          from els in Parse.String("else").Token()
                                                          from stmts in STMTS
                                                          select stmts;
            Parser<object[]> ELIF_STMTS = from els in Parse.String("elif")
                                                                                         from expr in IF_EXPR
                                                                                         from stmts in STMTS
                                                                                         select new object[] { expr, stmts };
            Parser<string> IfEnd = Parse.String("end").Text().Token();
            Parser<object[]> IF_STMT = 
                                                   from expr in IF_START
                                                   from stmts1 in STMTS
                                                   from stmts2 in
                                                       (from stmts in ELSE_STMTS
                                                        from c in IfEnd
                                                        select stmts).Or(
                                                                 from c in IfEnd
                                                                 select new object[] { })
                                                   select new object[]{
                                                                                    NodeType.FUNCALL,
                                                                                    new object[]{ 
                                                                                        "__if",
                                                                                        new object[]{
                                                                                            expr,
                                                                                            new object[]{ NodeType.BLOCK, new object[]{ new string[]{}, true, stmts1 } },
                                                                                            new object[]{ NodeType.BLOCK, new object[]{ new string[]{}, true, stmts2 } }
                                                                                        },
                                                                                        null
                                                                                    },
                                                                                    null
                                                                                };
            return IF_STMT;
        }

        static Parser<object[]> makeCaseStmtParser()
        {
            Parser<object[]> CASE_START = 
                                                    from _case in Parse.String("case").Text().Token()
                                                    from stmt in STMT
                                                    from t in Term
                                                    select stmt;
            Parser<object[]> WHEN_STMT = 
                                                      from a in Parse.String("when").Text().Token()
                                                      from expr in STMT
                                                      from t in Term
                                                      from stmts in STMTS
                                                      select new object[] {
                                                          expr,
                                                          new object[] {
                                                              NodeType.BLOCK,
                                                              new object[]{ new string[]{}, true, stmts }
                                                          }
                                                      };
            Parser<object[]> CASE_ELSE_STMTS = from _else in Parse.String("else").Token()
                                                                                                 from stmts in STMTS
                                                                                                 select new object[] {
                                                                                                        NodeType.BLOCK,
                                                                                                        new object[]{ new string[]{}, true, stmts }
                                                                                                 };
            Parser<string> CaseEnd = Parse.String("end").Text().Token();
            Parser<object[]> CASE_STMT1 = 
                                                   from expr in CASE_START
                                                   from _end in CaseEnd
                                                   select new object[]{
                                                       NodeType.FUNCALL,
                                                       new object[]{ 
                                                            "cond",
                                                            new object[]{
                                                                new object[] { NodeType.BOOL, "true" },
                                                                new object[] { NodeType.NL, "nl" },
                                                            },
                                                            null
                                                        }
                                                   };
            Parser<object[]> CASE_STMT2 =
                                                    from expr in CASE_START
                                                    from elseblock in CASE_ELSE_STMTS
                                                    from _end in CaseEnd
                                                    select makeCaseStmtObject(expr, null, elseblock);
            Parser<object[]> CASE_STMT3 =
                                                  from expr in CASE_START
                                                  from pairs in WHEN_STMT.Many()
                                                  from _end in CaseEnd
                                                  select makeCaseStmtObject(expr, pairs.ToList(), null);
            Parser<object[]> CASE_STMT4 = 
                                                   from expr in CASE_START
                                                   from pairs in WHEN_STMT.Many()
                                                   from elseblock in CASE_ELSE_STMTS
                                                   from _end in CaseEnd
                                                  select makeCaseStmtObject(expr, pairs.ToList(), elseblock);

            Parser<object[]> CASE_STMT = CASE_STMT1.Or(CASE_STMT2).Or(CASE_STMT3).Or(CASE_STMT4);
            return CASE_STMT;
        }

        static object[] makeCaseStmtObject(object[] expr, List<object[]> pairs, object[] elseblock)
        {
            List<object[]> pairs2 = null;
            if (pairs == null) {
                pairs2 = new List<object[]>();
            }
            else
            {
                pairs2 = pairs.Select((pair) => new object[]{
                                                                new object[]{
                                                                    NodeType.METHODS_CALL,
                                                                    new object[]{
                                                                        "==",
                                                                        expr,
                                                                        new object[]{ pair[0] },
                                                                        null
                                                                    },
                                                                    null
                                                                },
                                                                pair[1],
                                                            }).Aggregate(new List<object[]>(), (list, pair) =>
                                                           {
                                                               list.Add((object[])pair[0]);
                                                               list.Add((object[])pair[1]);
                                                               return list;
                                                           });
            }

            if (null != elseblock)
            {
                pairs2.Add(new object[] { NodeType.BOOL, "true" });
                pairs2.Add(elseblock);
            }

            return new object[] {
                        NodeType.FUNCALL,
                        new object[]{ 
                            "cond",
                            pairs2.ToArray(),
                            null
                        },
                        null
                    };
        }

        static Parser<object[]> makeExpressions(List<object[]> table, Parser<object[]> start)
        {
            foreach (object[] row in table.ToArray())
            {
                start = makeExpressionOperand(row, start);
            }
            return start;
        }

        static Parser<object[]> makeExpressionOperand(object[] row, Parser<object[]> start)
        {
            string[] operands = (string[])(row.Last());
            switch ((OperandType)(row.First()))
            {
                case OperandType.CHARIN_OPERATOR:
                    return makeChainOperator(operands, start);
                case OperandType.LEFT_UNARY:
                    return makeLeftUnary(operands, start);
                case OperandType.RIGHT_UNARY:
                    return makeRightUnary(operands, start);
                default:
                    return null;
            }
        }

        static Parser<object[]> makeChainOperator(string[] operators, Parser<object[]> beforeExpr)
        {
            return Parse.ChainOperator(
                operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2)),
                beforeExpr,
                (op, a, b) => new object[]{
                    NodeType.METHODS_CALL,
                    new object[]{ op, a, new object[]{b}, null }
                });
        }

        static Parser<object[]> makeLeftUnary(string[] operators, Parser<object[]> expr)
        {
            return (from h in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                        from v in expr
                        select new object[]{
                            NodeType.METHODS_CALL,
                            new object[]{ v, h+"@" },
                            null
                        }).Or(expr);
        }

        static Parser<object[]> makeRightUnary(string[] operators, Parser<object[]> expr)
        {
            return (from v in expr
                        from t in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                        select new object[]{
                               NodeType.METHODS_CALL,
                               new object[]{ v, "@"+t },
                               null
                        }).Or(expr);
        }

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

        public static void regenerateParser()
        {
            PRIMARY_PARSERS = new List<Parser<object[]>>() { NL, NUMERIC, BOOL, SYMBOL, STRING, ARRAY, HASH, BLOCK, LAMBDA };
            PRIMARY = makePrimaryParser();
            EXPR_PARSERS = new List<Parser<object[]>>() { EXP_EQUAL };
            EXPR = makeExprParser();
            METHODS_CALL = makeMethodsCallParser();
            STMT_PARSERS = new List<Parser<object[]>>() { DEFINE_CLASS, DEFINE_FUNCTION, CASE_STMT, IF_STMT, EXPR };
            STMT = makeStmtParser();
            STMTS = makeStmtsParser();
            PROGRAM = makeProgramParser();
        }

        public static Object.LpObject execute(string ctx)
        {
            // IInputを使って、行番号を取得できるパーサーを使ってみるテスト。

            regenerateParser();
            /*
            var pobj = PROGRAM.Parse(ctx);
            Console.WriteLine(pobj);
            var node = toNode(pobj);
            Console.WriteLine(node);
            */

            var node = createNode(ctx);
            var o = node.Evaluate();

            return o;
        }
    }
}
