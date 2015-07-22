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

        // Expressions
        static readonly List<object[]> operandTable = new List<object[]> {
            new object[]{ 3, new string[]{ "++", "--" } },
            new object[]{ 2, new string[]{ "+", "!", "~" } },
            new object[]{ 2, new string[]{ "not" } },
            new object[]{ 1, new string[]{ "**" } },
            new object[]{ 2, new string[]{ "-" } },
            new object[]{ 1, new string[]{ "*","/", "%" } },
            new object[]{ 1, new string[]{ "+","-" } },
            new object[]{ 1, new string[]{ "<<",">>" } },
            new object[]{ 1, new string[]{ "&" } },
            new object[]{ 1, new string[]{ "|" } },
            new object[]{ 1, new string[]{ ">=", ">", "<=", "<" } },
            new object[]{ 1, new string[]{ "<=>", "===", "==", "!=", "=~", "!~" } },
            new object[]{ 1, new string[]{ "&&" } },
            new object[]{ 1, new string[]{ "||" } },
            new object[]{ 1, new string[]{ "..", "^..", "..^", "^..^" } },
            new object[]{ 1, new string[]{ "and", "or" } }
        };


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

        static readonly Parser<string> String = Parser.StringParser.String;

        // Array, Hash
        static readonly Parser<string> SepElm = (from sep in Parse.Char(',').Token()
                                                 from s in Parse.Ref(() => Stmt)
                                                 select s).Or(Parse.Ref(() => Stmt));

        static readonly Parser<string> Array = from elms in SepElm.Many().Contained(Parse.String("[").Text().Token(), Parse.String("]").Text().Token())
                                               select "[" + string.Join(",", elms) + "]";

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

        static readonly Parser<string> Symbol = from qmark in Parse.String(":").Text()
                                                from idf in TotalVarname
                                                select qmark + idf;

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

        static readonly Parser<string> ExpVal = (from a in Parse.Char('(')
                                                 from v in Expr
                                                 from b in Parse.Char(')')
                                                 select "(" + v + ")").Or(Primary).Token();
        // ::
        static readonly Parser<string> Classcall = OperandsChainCallStart(Parse.String("::"), ExpVal, Funcall.Or(Funcall0), (opr, a, b) => a + opr + b);

        static readonly Parser<string> ExpClasscall = Classcall.Or(ExpVal);

        // .
        static readonly Parser<string> MethodCall = OperandsChainCallStart(Parse.String(".").Text(), ExpClasscall, Funcall.Or(Funcall0), (opr, a, b) => a + opr + b);
        static readonly Parser<string> ExpMethodcall = MethodCall.Or(ExpClasscall);
        // []
        static readonly Parser<string> ExpArrayAt = (from expr in ExpMethodcall
                                                     from v in Stmt.Contained(Parse.Char('['), Parse.Char(']'))
                                                     select string.Format("{0}.([])({1})", expr, v)).Or(ExpMethodcall);
        // ２項演算子一覧
        static readonly Parser<string> ChainExprs = makeExpressions(operandTable, ExpArrayAt);
        // =(+=, -= ... )
        // TODO: 代入演算子作成
        static readonly Parser<string> ExpAssignment = ChainExprs;
        // =
        static readonly Parser<string> ExpEqualRaw = from vname in Varname.Or(GlobalVarname).Or(InstanceVarname).Token()
                                                     from eq in Parse.String("=").Text().Token()
                                                     from v in ExpAssignment.Token()
                                                     select string.Format(":{0}.({1})({2})", vname, eq, v);
        static readonly Parser<string> ExpEqual = ExpEqualRaw.Or(ExpAssignment);
        // 演算子一覧
        static readonly Parser<string> Expr = ExpEqual;

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
        static readonly Parser<string> AstArg = from ast in Parse.Char('*')
                                                from id in Varname
                                                select ast + id;
        static readonly Parser<string> AmpArg = from amp in Parse.Char('&')
                                                from id in Varname
                                                select amp + id;

        static readonly Parser<string> ExpEqualRawDecl = from vname in TotalVarname
                                                         from eq in Parse.String("=").Text().Token()
                                                         from v in ExpAssignment.Token()
                                                         select string.Format("{0}{1}{2}", vname, eq, v);
        static readonly Parser<string[]> SimpleArgList = from args in Parse.DelimitedBy(ExpEqualRawDecl.Or(Varname), Parse.Char(',').Token())
                                                         select args.ToArray();
        static readonly Parser<string[]> AstArgs = from arg in AstArg.Text()
                                                   select new string[] { arg };
        static readonly Parser<string[]> AmpArgs = from arg in AmpArg.Text()
                                                   select new string[] { arg };
        static readonly Parser<string[]> WithAmpArgs = from args in SimpleArgList
                                                       from dtr in Parse.Char(',').Token()
                                                       from amparg in AmpArg.Text()
                                                       select args.Concat(new string[] { amparg }).ToArray();
        static readonly Parser<string[]> WithAstArgs = from args in SimpleArgList
                                                       from dtr in Parse.Char(',').Token()
                                                       from astarg in AstArg.Text()
                                                       select args.Concat(new string[] { astarg }).ToArray();
        static readonly Parser<string[]> WithAstAmpArgs = from args in SimpleArgList
                                                          from dtr1 in Parse.Char(',').Token()
                                                          from astarg in AstArg.Text()
                                                          from dtr2 in Parse.Char(',').Token()
                                                          from amparg in AmpArg.Text()
                                                          select args.Concat(new string[] { astarg, amparg }).ToArray();
        static readonly Parser<string[]> ArgList = new Parser<string[]>[] { WithAstAmpArgs, WithAmpArgs, WithAstArgs, AmpArgs, AstArgs, SimpleArgList, ZeroArgs }.Aggregate((seed, nxt) => seed.Or(nxt));
        static readonly Parser<string[]> ArgDecl = ArgList.Contained(Parse.Char('('), Parse.Char(')'));

        // is Statements
        static readonly Parser<string> IfExpr = Parse.Ref(() => Stmt.Contained(Parse.Char('('), Parse.Char(')')).Or(
                                                                  from stmt in Stmt
                                                                  from b in Term
                                                                  select stmt));
        static readonly Parser<string> IfStart = (from _if in Parse.String("if")
                                                  from expr in IfExpr
                                                  select expr).Token();
        static readonly Parser<string[]> ElseStmts = from els in Parse.String("else").Token()
                                                     from stmts in Stmts
                                                     select stmts;
        static readonly Parser<string[][]> ElIfStmts = from els in Parse.String("elif")
                                                       from expr in IfExpr
                                                       from stmts in Stmts
                                                       select new string[][] { new string[] { expr }, stmts };
        static readonly Parser<string> IfEnd = Parse.String("end").Text().Token();
        static readonly Parser<string> IfStmt = from expr in IfStart
                                                from stmts1 in Stmts
                                                from stmts2 in
                                                    (from stmts in ElseStmts
                                                     from c in IfEnd
                                                     select stmts).Or(
                                                     from c in IfEnd
                                                     select new string[] { })
                                                select string.Format(
                                                    "__if({0},do {1} end,do {2} end)",
                                                    expr,
                                                    string.Join("; ", stmts1),
                                                    string.Join("; ", stmts2));
        
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

        internal static readonly Parser<string> Stmt = (from s in StatList
                                               from t in Term
                                               select s).Or(StatList).Token().Except(Parse.String("end").Or(Parse.String(")")).Token());

        internal static readonly Parser<string[]> Stmts = from stmts in Stmt.XMany()
                                                 select stmts.ToArray();

        static readonly Parser<string> Program = from stmts in Stmts
                                                 select string.Join("; ", stmts.ToArray());

        static readonly Parser<object[]> STRING = from a in Parse.Char('"')
                                                  from s in EscapeSequence.Or(makeEscapePerser('"', '\"')).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                  from b in Parse.Char('"')
                                                  select new object[] { NodeType.STRING, string.Join("", s.ToArray()) };
        static readonly Parser<object[]> VARIABLE_CALL = from varname in Varname
                                                         select new object[] { NodeType.VARIABLE_CALL, varname };
        static readonly Parser<object[]> INSTANCE_VARIABLE_CALL = from h in Parse.String("@")
                                                                  from varname in Varname
                                                                  select new object[] { NodeType.INSTANCE_VARIABLE_CALL, varname };
        static readonly Parser<object[]> GLOBAL_VARIABLE_CALL = from h in Parse.String("$")
                                                                from varname in Varname
                                                                select new object[] { NodeType.GLOBAL_VARIABLE_CALL, varname };
        static readonly Parser<object[]> ARRAY = from a in Parse.String("[").Text().Token()
                                                 from elms in SepElm.Many()
                                                 from b in Parse.String("]").Text().Token()
                                                 select new object[] { NodeType.ARRAY, elms.Select((elm) => STMT.Parse(elm)).ToList() };
        static readonly Parser<object[]> BLOCK_START1 = from a in Parse.String("do").Token()
                                                        select new object[] { new string[] { }, false };
        static readonly Parser<object[]> BLOCK_START2 = from _do in Parse.String("do").Token()
                                                        from args in FenceArgs.Token()
                                                        select new object[] { args, true };
        static readonly Parser<object[]> BLOCK_START3 = from args in BlaketArgs.Token()
                                                        from _do in Parse.String("do").Token()
                                                        select new object[] { args, false };
        static readonly Parser<object[]> BLOCK_START = BLOCK_START3.Or(BLOCK_START2).Or(BLOCK_START1);
        static readonly Parser<object[]> BLOCK_STMT = from argset in BLOCK_START
                                                      from stmts in STMTS
                                                      from b in Parse.String("end").Token()
                                                      select new object[] { (string[])argset[0], (bool)argset[1], stmts };

        static readonly Parser<object[]> LAMBDA = from head in Parse.String("->").Token()
                                                  from blk in BLOCK_STMT.Token()
                                                  select new object[] { NodeType.LAMBDA, blk };

        static readonly Parser<object[]> BLOCK = from blk in BLOCK_STMT.Token()
                                                 select new object[] { NodeType.BLOCK, blk };

        static readonly Parser<object[]> HASH = from a in Parse.String("{").Text().Token()
                                                from pairs in Assoc.Many()
                                                from b in Parse.String("}").Text().Token()
                                                select makeHash(pairs.ToArray());

        static readonly Parser<object[]> QUOTE = from m in Parse.String("'").Text()
                                                 from stmt in STMT
                                                 select new object[] { NodeType.QUOTE, toNode(stmt).toSource() };

        static readonly Parser<object[]> QUASI_QUOTE = from m in Parse.String("`").Text()
                                                       from stmt in STMT
                                                       select new object[] { NodeType.QUASI_QUOTE, toNode(stmt).toSource() };
        static readonly Parser<object[]> QUESTION_QUOTE = from m in Parse.String("?").Text()
                                                          from stmt in ExpVal
                                                          select new object[] { NodeType.QUESTION_QUOTE, stmt };

        public static readonly Parser<object[]> PRIMARY = new Parser<object[]>[] { NL, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, HASH, LAMBDA, BLOCK, QUOTE, QUASI_QUOTE, QUESTION_QUOTE }.Aggregate((seed, nxt) => seed.Or(nxt));

        static readonly Parser<object[]> EXP_VAL = (from a in Parse.Char('(').Token()
                                                    from s in STMT
                                                    from b in Parse.Char(')').Token()
                                                    select s).Or(PRIMARY);
        static readonly Parser<object[]> ARGS = from args in Args
                                                select args.Select((arg) => (object[])STMT.Parse(arg)).ToArray();
        static readonly Parser<object[]> ARGS_CALL = from a in Parse.Char('(').Token()
                                                     from args in Args
                                                     from b in Parse.Char(')').Token()
                                                     select args.Select((arg) => ((object[])STMT.Parse(arg))).ToArray();
        static readonly Parser<object[]> METHOD_CALL = (from fname in Fname
                                                        from args in ARGS_CALL
                                                        from blk in BLOCK.Token()
                                                        select new object[] { fname, args, blk }).Or(
                                                        from fname in Fname
                                                        from args in ARGS_CALL
                                                        select new object[] { fname, args, null });
        static readonly Parser<object[]> FUNCTION_CALL = from fvals in METHOD_CALL
                                                         select new object[] { NodeType.FUNCTION_CALL, fvals };
        static readonly Parser<object[]> STARTER = new Parser<object[]>[] { EXP_VAL, FUNCTION_CALL, GLOBAL_VARIABLE_CALL, INSTANCE_VARIABLE_CALL, VARIABLE_CALL }.Aggregate((seed, nxt) => seed.Or(nxt));

        static readonly Parser<object[]> FUNCALL = OperandsChainCallStart(Parse.String("."), STARTER, METHOD_CALL, (dot, op1, op2) =>
        {
            return new object[] {
                NodeType.FUNCALL,
                new object[]{
                    (string)op2[0],
                    op1,
                    (object[])op2[1],
                    (object[])op2[2]
                }
            };
        });
        
        static readonly Parser<object[]> EXPR = FUNCALL.Or(STARTER).Token();
        static readonly Parser<object[]> STMT = (from expr in EXPR
                                                 from t in Term
                                                 select expr).Or(EXPR).Token();
        static readonly Parser<object[]> STMTS = from stmts in STMT.Many().Token()
                                                 select new object[] { NodeType.STMTS, stmts };
        public static readonly Parser<object[]> PROGRAM = STMTS;

        static Parser<T> OperandsChainCallStart<T, T2, TOp>(
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

        static Parser<string> makeExpressionOperand(object[] row, Parser<string> start)
        {
            string[] operands = (string[])(row.Last());
            switch ((int)(row.First()))
            {
                case 1:
                    return makeChainOperator(operands, start);
                case 2:
                    return makeLeftUnary(operands, start);
                case 3:
                    return makeRightUnary(operands, start);
                default:
                    return null;
            }
        }

        static Parser<string> makeExpressions(List<object[]> table, Parser<string> start)
        {
            foreach (object[] row in table.ToArray())
            {
                start = makeExpressionOperand(row, start);
            }
            return start;
        }

        static Parser<string> makeChainOperator(string[] operators, Parser<string> beforeExpr)
        {
            return Parse.ChainOperator(
                operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2)),
                beforeExpr,
                (op, a, b) => string.Format("{0}.({1})({2})", a, op, b));
        }

        static Parser<string> makeLeftUnary(string[] operators, Parser<string> expr)
        {
            return (from h in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                    from v in expr
                    select string.Format("{0}.({1}@)()", v, h)).Or(expr);
        }

        static Parser<string> makeRightUnary(string[] operators, Parser<string> expr)
        {
            return (from v in expr
                    from t in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                    select string.Format("{0}.(@{1})()", v, t)).Or(expr);
        }

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

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
                    return Ast.LpAstLeaf.toNode((string)node[1], "QUOTE");
                case NodeType.QUASI_QUOTE:
                    return Ast.LpAstLeaf.toNode((string)node[1], "QUASI_QUOTE");
                case NodeType.QUESTION_QUOTE:
                    return Ast.LpAstLeaf.toNode((string)node[1], "QUESTION_QUOTE");
                case NodeType.FUNCTION_CALL:
                    return Ast.LpAstFuncall.toNode((object[])node[1]);
                case NodeType.LAMBDA:
                    return Ast.LpAstLambda.toNode((object[])node[1]);
                case NodeType.BLOCK:
                    return Ast.LpAstBlock.toNode((object[])node[1]);
                case NodeType.FUNCALL:
                    return Ast.LpAstMethodCall.toNode((object[])node[1]);
                case NodeType.EXPR:
                    return toNode((object[])node[1]);
                case NodeType.STMTS:
                    return Ast.LpAstStmts.toNode((List<object[]>)node[1]);
                case NodeType.ARRAY:
                    return Ast.LpAstArray.toNode((List<object[]>)node[1]);
                case NodeType.HASH:
                    return Ast.LpAstHash.toNode((object[])node[1]);
                default:
                    return null;
            }
        }

        public static Ast.LpAstNode createNode(string ctx)
        {
            var str = Sprache.ParserExtensions.Parse(Program, ctx);

            Console.WriteLine(str);
            var pobj = PROGRAM.Parse(str);
            var node = toNode(pobj);
            return node;
        }
    }
}
