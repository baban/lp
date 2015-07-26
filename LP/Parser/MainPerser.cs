﻿using System;
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
        // Array, Hash
        static readonly Parser<string[]> AssocVal = from k in Parse.Ref(() => Stmt)
                                                    from sps in Parse.Char(':').Token()
                                                    from s in Parse.Ref(() => Stmt)
                                                    select new string[] { k, s };

        static readonly Parser<string[]> Assoc = (from sep in Parse.Char(',').Token()
                                                  from kv in AssocVal
                                                  select kv).Or(AssocVal);

        static readonly Parser<string[]> Assocs = from as1 in AssocVal
                                                  from ass in
                                                      (from dt in Parse.Char(',').Token()
                                                       from pas in AssocVal
                                                       select pas).Many()
                                                  select new string[][] { as1 }.Concat(ass).Select((pair) => pair[0] + " : " + pair[1]).ToArray();

        static readonly Parser<string> Hash = from a in Parse.String("{").Text().Token()
                                              from pairs in Assoc.Many()
                                              from b in Parse.String("}").Text().Token()
                                              select a + string.Join(",", pairs.Select((pair) => pair[0] + " : " + pair[1])) + b;

        // Macro Values
        static readonly Parser<string> Quote = from qmark in Parse.String("'").Text()
                                               from idf in Expr
                                               select qmark + idf;

        static readonly Parser<string> QuasiQuote = from qmark in Parse.String("`").Text()
                                                    from idf in Expr
                                                    select qmark + idf;

        static readonly Parser<string> QuestionQuote = from qmark in Parse.String("?").Text()
                                                       from idf in ExpVal
                                                       select qmark + idf;

        // Block,Lambda
        static readonly Parser<string[]> BlaketArgs = Parse.Ref(() => ArgList).Contained(Parse.Char('('), Parse.Char(')'));
        static readonly Parser<string[]> FenceArgs = Parse.Ref(() => ArgList).Contained(Parse.Char('|'), Parse.Char('|'));

        static readonly Parser<string> BlockStart1 = from _do in Parse.String("do").Text()
                                                     from ws in Parse.WhiteSpace.Many()
                                                     select _do;

        static readonly Parser<string> BlockStart2 = from args in BlaketArgs
                                                     from _do in Parse.String("do").Token()
                                                     select string.Format("({0}) do", string.Join(", ", args));
        static readonly Parser<string> BlockStart3 = from _do in Parse.String("do")
                                                     from ws in Parse.WhiteSpace.Many()
                                                     from args in FenceArgs.Token()
                                                     select string.Format("do |{0}|", string.Join(", ", args));
        static readonly Parser<string> BlockStart = BlockStart3.Or(BlockStart2).Or(BlockStart1);
        static readonly Parser<string> BlockStmt = from start in BlockStart
                                                   from stmts in Parse.Ref(() => Stmts)
                                                   from c in Parse.String("end").Token()
                                                   select string.Format("{0} {1} end", start, string.Join("; ", stmts.ToArray()));
        static readonly Parser<string> Block = BlockStmt;
        static readonly Parser<string> Lambda = from a in Parse.String("->").Text()
                                                where a.ToString() == "->"
                                                from blk in Block
                                                select "->" + blk;
        static readonly Parser<string> Function = from a in Parse.String("def").Token()
                                                  from fname in Fname
                                                  from args in ArgDecl
                                                  from b in Term
                                                  from stmts in Stmts
                                                  from c in Parse.String("end")
                                                  select string.Format("->({0}) do {1} end.bind(:{2})", string.Join(", ", args.ToArray()),
                                                  string.Join("; ", stmts.ToArray()), fname);
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

        static readonly Parser<string> DefClass = from a in Parse.String("class").Token().Text()
                                                  from cname in Fname
                                                  from b in Term
                                                  from stmts in Stmt.Many().Except(Parse.String("end"))
                                                  from c in Parse.String("end").Token()
                                                  select string.Format("Class.new(:{1}) do {0} end",
                                                                    string.Join("; ", stmts), cname);

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

        static readonly Parser<string> Funcall = from idf in Fname
                                                 from args in Args.Contained(Parse.Char('(').Once(), Parse.Char(')').Once())
                                                 from blk in Block.Token().Optional()
                                                 select string.Format("{0}({1}){2}", idf, string.Join(", ", args.ToArray()), (blk.IsEmpty ? "" : " " + blk.Get()));

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
        
        static readonly Parser<string> CaseStart = (from _case in Parse.String("case").Text().Token()
                                                    from stmt in Stmt
                                                    select stmt).Token();
        static readonly Parser<string[][]> WhenStmt = from a in Parse.String("when").Text().Token()
                                                      from expr in Stmt
                                                      from stmts in Stmts
                                                      select new string[][] { new string[] { expr }, stmts.ToArray() };
        static readonly Parser<string[]> CaseElseStmts = from _else in Parse.String("else").Token()
                                                         from stmts in Stmts
                                                         select stmts;
        static readonly Parser<string> CaseEnd = Parse.String("end").Text().Token();
        static readonly Parser<string> CaseStmt1 = from _start in CaseStart
                                                   from _end in CaseEnd
                                                   select string.Format("cond( {0} )", _start);
        static readonly Parser<string> CaseStmt2 = from _start in CaseStart
                                                   from elsestmts in CaseElseStmts
                                                   from _end in CaseEnd
                                                   select string.Format(
                                                     "cond( {0}, true, do {1} end )",
                                                     _start,
                                                     string.Join("; ", elsestmts));
        static readonly Parser<string> CaseStmt3 = from _start in CaseStart
                                                   from pairs in WhenStmt.Many()
                                                   from _end in CaseEnd
                                                   select string.Format(
                                                    "cond( {0}, {1} )",
                                                    _start,
                                                    string.Join(", ",
                                                        pairs.Select((pair) =>
                                                        {
                                                            var expr = pair[0].First();
                                                            var stmts = pair[1];
                                                            return string.Format("{0}, do {1} end", expr, string.Join("; ", stmts));
                                                        })));
        static readonly Parser<string> CaseStmt4 = from _start in CaseStart
                                                   from pairs in WhenStmt.Many()
                                                   from elsestmt in CaseElseStmts
                                                   from _end in CaseEnd
                                                   select string.Format(
                                                    "cond( {0}, {1}, true, do {2} end )",
                                                    _start,
                                                    string.Join(", ",
                                                        pairs.Select((pair) =>
                                                        {
                                                            var expr = pair[0].First();
                                                            var stmts = pair[1];
                                                            return string.Format("{0}, do {1} end", expr, string.Join("; ", stmts));
                                                        })),
                                                    string.Join("; ", elsestmt));
        static readonly Parser<string> CaseStmt = CaseStmt1.Or(CaseStmt2).Or(CaseStmt3).Or(CaseStmt4);

        static readonly Parser<string> StatList = DefMacro.Or(DefClass).Or(DefModule).Or(Function).Or(IfStmt).Or(CaseStmt).Or(Expr);

        static readonly Parser<object[]> QUASI_QUOTE = from m in Parse.String("`").Text()
                                                       from stmt in STMT
                                                       select new object[] { NodeType.QUASI_QUOTE, toNode(stmt).toSource() };
        static readonly Parser<object[]> QUESTION_QUOTE = from m in Parse.String("?").Text()
                                                          from stmt in ExpVal
                                                          select new object[] { NodeType.QUESTION_QUOTE, stmt };
        public static readonly Parser<object[]> PRIMARY = new Parser<object[]>[] { NL, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, HASH, LAMBDA, BLOCK, QUOTE, QUASI_QUOTE, QUESTION_QUOTE }.Aggregate((seed, nxt) => seed.Or(nxt));


        static Object.LpObject defClass(string fname, List<Ast.LpAstNode> stmts)
        {
            var o = Object.LpKernel.initialize();
            o.methods[fname] = Object.LpClass.initialize(fname, stmts);
            return (Object.LpObject)o.methods[fname];
        }

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

        // 単体テスト時にアクセスしやすいように
        static object[] parseNode(Parser<object[]> psr, string ctx)
        {
            return psr.Parse(ctx);
        }

        // 単体テスト時にアクセスしやすいように
        static Ast.LpAstNode parseToNode(Parser<object[]> psr, string ctx)
        {
            var p = psr.Parse(ctx);
            //Console.WriteLine("p");
            //Console.WriteLine(p);        
            var o = toNode(p);
            //Console.WriteLine("o");
            //Console.WriteLine(o);
            return o;
        }

        // 単体テスト時にアクセスしやすいように
        static Object.LpObject parseToObject(Parser<object[]> psr, string ctx)
        {
            Console.WriteLine("parseToObject");
            var p = psr.Parse(ctx);
            Console.WriteLine("p");
            Console.WriteLine(p);
            var o = toNode(p);
            Console.WriteLine("o");
            Console.WriteLine(o);
            return o.Evaluate();
        }

        public static Object.LpObject parseStmtObject(string ctx)
        {
            var p = STMT.Parse(ctx);
            var o = toNode(p);
            return o.Evaluate();
        }

        static Object.LpObject parseArrObject(Parser<Object.LpObject> psr, string ctx)
        {
            Console.WriteLine(ctx);
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

        static readonly Parser<object[]> DEFINE_FUNCTION = makeDefineFunctionParser();
        static readonly Parser<object[]> IF_STMT = makeIfStmtParser();
        static readonly Parser<object[]> CASE_STMT = makeCaseStmtParser();

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
            STMT_PARSERS = new List<Parser<object[]>>() { DEFINE_FUNCTION, CASE_STMT, IF_STMT, EXPR };
            STMT = makeStmtParser();
            STMTS = makeStmtsParser();
            PROGRAM = makeProgramParser();
        }

        public static Object.LpObject execute(string ctx)
        {
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
