using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using System.Diagnostics;

namespace LP
{
    // TODO: 引数の改良
    // TODO: ハッシュ作成
    // TODO: case文
    // TODO: メソッド定義
    // TODO: class定義、module定義
    // TODO: 構文エラー処理
    // TODO: エラー処理
    // TODO: Block読み出し
    // TODO: インスタンス変数
    // TODO: グローバル変数
    // TODO: コメントはできるだけあとに残す
    class LpParser
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

        static readonly Parser<string> OperandMarks = new string[] { "**", "*", "/", "%", "+", "-", "<<", ">>", "&", "|", ">=", ">", "<=", "<", "<=>", "===", "==", "!=", "=~", "!~", "&&", "||", "and", "or", "=" }.Select(op => Parse.String(op)).Aggregate((op1, op2) => op1.Or(op2)).Text();

        // 基本文字一覧
        static readonly Parser<string> Term = Parse.Regex("^[;\n]");

        static readonly Parser<string> ReservedNames = new string[] { "true", "false", "do", "end", "else", "def" }.Select( (s) => Parse.String(s).Text() ).Aggregate((seed, nxt) => seed.Or(nxt)).Token();

        // Primary Values
        static readonly Parser<string> Identifier = Parse.Except(Parse.Regex("[a-zA-Z_][a-zA-Z0-9_]*"), ReservedNames );

        static readonly Parser<string> Fname = (from a in Parse.String("(").Text()
                                                from v in OperandMarks.Or(Identifier)
                                                from b in Parse.String(")").Text()
                                                select a + v + b).Or(Identifier);

        static readonly Parser<string> Varname = Identifier;

        static readonly Parser<string> Nl = Parse.Regex("nl").Named("Nl");
        static readonly Parser<string> Bool = Parse.Regex("true|false").Named("boolean");
        static readonly Parser<string> Decimal = Parse.Regex(@"\d+\.\d+");
        static readonly Parser<string> Int = Parse.Regex(@"\d+");
        static readonly Parser<string> Numeric = Decimal.Or(Int).Named("numeric");
        // TODO: 変数展開実装
        static readonly Parser<string> String = (from a in Parse.Char('"')
                                                 from s in ( Parse.Char('\\').Once().Concat( Parse.Char('"').Once() )).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                 from b in Parse.Char('"')
                                                 select '"' + string.Join("", s.ToArray()) + '"').Named("string");
        // Comment
        static readonly Parser<string> InlineComment = Parse.Regex("//.*?\n").Return("").Named("lnline comment");
        static readonly Parser<string> BlockComment = Parse.Regex(@"/\*.*?\*/").Return("").Named("block comment");

        static readonly Parser<string> Comment = InlineComment.Or(BlockComment);

        // Array, Hash
        static readonly Parser<string> SepElm = (from sep in Parse.Char(',').Token()
                                                 from s in Parse.Ref(() => Stmt)
                                                 select s).Or(Parse.Ref(() => Stmt));

        static readonly Parser<string> Array = from elms in SepElm.Many().Contained(Parse.String("[").Text().Token(),Parse.String("]").Text().Token())
                                               select "[" + string.Join(",", elms) + "]";

        static readonly Parser<string[]> AssocVal = from k in Parse.Ref(() => Stmt)
                                                    from sps in Parse.Char(':').Token()
                                                    from s in Parse.Ref(() => Stmt)
                                                    select new string[]{ k, s };

        static readonly Parser<string[]> Assoc = (from sep in Parse.Char(',').Token()
                                                  from kv in AssocVal
                                                  select kv).Or(AssocVal);

        static readonly Parser<string> Hash = from a in Parse.String("{").Text().Token()
                                              from pairs in Assoc.Many()
                                              from b in Parse.String("}").Text().Token()
                                              select a + string.Join(",", pairs.Select((pair) => pair[0] + " : " + pair[1])) + b;

        static readonly Parser<string> Symbol = from qmark in Parse.String(":").Text()
                                                from idf in Varname
                                                select qmark+idf;

        // Macro Values
        static readonly Parser<string> Quote = from qmark in Parse.String("'").Text()
                                               from idf in Expr
                                               select qmark + idf;
        static readonly Parser<string> QuasiQuote =  from qmark in Parse.String("`").Text()
                                                     from idf in Expr
                                                     select qmark+idf;
        static readonly Parser<string> QuestionQuote = from qmark in Parse.String("?").Text()
                                                       from idf in Stmt
                                                       select qmark+idf;
        // Block,Lambda
        static readonly Parser<string[]> BlaketArgs = Parse.Ref(() => ArgList).Contained(Parse.Char('('),Parse.Char(')'));
        static readonly Parser<string[]> FenceArgs = Parse.Ref(() => ArgList).Contained(Parse.Char('|'),Parse.Char('|'));

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
        static readonly Parser<string> BlockStmt =  from start in BlockStart
                                                    from stmts in Parse.Ref(() => Stmts )
                                                    from c in Parse.String("end").Token()
                                                    select string.Format("{0} {1} end", start, string.Join("; ", stmts.ToArray() ) );
        static readonly Parser<string> Block = BlockStmt;
        static readonly Parser<string> Lambda = from a in Parse.String("->").Text()
                                                where a.ToString()=="->"
                                                from blk in Block
                                                select "->" + blk;
        static readonly Parser<string> Function = from a in Parse.String("def").Token()
                                                  from fname in Fname
                                                  from args in ArgDecl
                                                  from b in Term
                                                  from stmts in Stmts
                                                  from c in Parse.String("end")
                                                  select string.Format("->({0}) do {1} end.bind(:{2})", string.Join(", ", args.ToArray()), 
                                                  string.Join("; ", stmts.ToArray()), fname );

        static readonly Parser<string> DefClass = from a in Parse.String("class").Token().Text()
                                                  from cname in Fname
                                                  from b in Term
                                                  from stmts in Stmt.Many().Except(Parse.String("end"))
                                                  from c in Parse.String("end").Token()
                                                  select string.Format("Class.new(:{1}) do {0} end",
                                                                    string.Join("; ", stmts),cname);

        static readonly Parser<string> Funcall0 = from idf in Fname
                                                  select idf + "()";
        static readonly Parser<string> Varcall = Varname.Token();

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
        static readonly Parser<string> MethodCall = OperandsChainCallStart(Parse.String(".").Text(), ExpClasscall, Funcall.Or(Funcall0), (opr, a, b) => a + opr + b );
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
        static readonly Parser<string> ExpEqual = (from vname in Varname.Token()
                                                   from eq in Parse.String("=").Text().Token()
                                                   from v in ExpAssignment.Token()
                                                   select string.Format(":{0}.({1})({2})", vname, eq, v )).Or(ExpAssignment);
        // 演算子一覧
        static readonly Parser<string> Expr = ExpEqual;

        // arguments
        static readonly Parser<string> Arg = Parse.Ref(() => Stmt);
        static readonly Parser<string[]> ZeroArgs = from sps in Parse.WhiteSpace.Many()
                                                    select new string[]{};
        static readonly Parser<string[]> Args = from ags in Parse.DelimitedBy(Arg, Parse.Char(',')).Or(ZeroArgs)
                                                select ags.ToArray();
        static readonly Parser<string[]> ArgsCall = from a in Parse.Char('(')
                                                    from args in Args
                                                    from b in Parse.Char(')')
                                                    select args.ToArray();
        static readonly Parser<string> AstArg = Parse.Char('*').Once().Then((ast) => from id in Varname
                                                                                     select ast + id);
        static readonly Parser<string> AmpArg = Parse.Char('&').Once().Then( (ast) => from id in Varname
                                                                                      select ast+id );
        static readonly Parser<IEnumerable<string>> SimpleArgList = Parse.DelimitedBy(Varname, Parse.Char(',').Token());
        static readonly Parser<string[]> ArgList = from ags in
                                                       (from args in SimpleArgList
                                                        from amparg in AmpArg
                                                        select args.Concat(new string[] { amparg })).Or(
                                                        from args in SimpleArgList
                                                        from amparg in AmpArg
                                                        select args.Concat(new string[] { amparg })).Or(
                                                        from args in SimpleArgList
                                                        from astarg in AstArg
                                                        select args.Concat(new string[] { astarg })).Or(SimpleArgList).Or(ZeroArgs)
                                                   select ags.ToArray();
        static readonly Parser<string[]> ArgDecl = ArgList.Contained(Parse.Char('('), Parse.Char(')'));

        // is Statements
        static readonly Parser<string> IfExpr =  Parse.Ref( () => Stmt.Contained(Parse.Char('('),Parse.Char(')')).Or(
                                                                  from stmt in Stmt
                                                                  from b in Term
                                                                  select stmt) );
        static readonly Parser<string> IfStart = (from _if in Parse.String("if")
                                                  from expr in IfExpr
                                                  select expr).Token();
        static readonly Parser<string[]> ElseStmts = from els in Parse.String("else").Token()
                                                     from stmts in Stmts
                                                     select stmts;
        static readonly Parser<string[][]> ElIfStmts = from els in Parse.String("elif")
                                                       from expr in IfExpr
                                                       from stmts in Stmts
                                                       select new string[][]{ new string[]{ expr }, stmts };
        static readonly Parser<string> IfEnd = Parse.String("end").Text().Token();
        static readonly Parser<string> IfStmt = from expr in IfStart
                                                from stmts1 in Stmts
                                                from stmts2 in (from stmts in ElseStmts
                                                                from c in IfEnd
                                                                select stmts).Or(
                                                                from c in IfEnd
                                                                select new string[]{})
                                                select string.Format("_if({0},do {1} end,do {2} end)", expr, string.Join("; ", stmts1), string.Join("; ", stmts2));

        static readonly Parser<string> StatCollection = DefClass.Or(Function).Or(IfStmt);
        static readonly Parser<string> StatList = StatCollection.Or(Expr);

        static readonly Parser<string> Stmt = (from s in StatList
                                               from t in Term
                                               select s).Or(StatList).Token().Except(Parse.String("end").Or(Parse.String(")")).Token());

        static readonly Parser<string[]> Stmts = from stmts in Stmt.XMany()
                                                 select stmts.ToArray();

        static readonly Parser<string> Program = from stmts in Stmts
                                                 select string.Join( "; ", stmts.ToArray() );
        public enum NodeType {
            NL, INT, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, VARIABLE_CALL, LAMBDA, BLOCK, PRIMARY,
            ARGS, FUNCALL, EXPR, EXP_VAL, FUNCTION_CALL, STMT, STMTS, PROGRAM, HASH, QUOTE, QUASI_QUOTE, QUESTION_QUOTE
        };
        static readonly Parser<object[]> NL = from s in Nl
                                                select new object[] { NodeType.NL, s };
        static readonly Parser<object[]> INT = from n in Int
                                               select new object[] { NodeType.INT, n };
        static readonly Parser<object[]> NUMERIC = from n in Numeric
                                                   select new object[] { NodeType.NUMERIC, n };
        static readonly Parser<object[]> BOOL = from b in Bool
                                                select new object[] { NodeType.BOOL, b };
        static readonly Parser<object[]> STRING = from a in Parse.Char('"')
                                                  from s in Parse.CharExcept('"').Many().Text()
                                                  from b in Parse.Char('"')
                                                  select new object[]{ NodeType.STRING, s };
        static readonly Parser<object[]> SYMBOL = from m in Parse.String(":").Text()
                                                  from s in Identifier
                                                  select new object[] { NodeType.SYMBOL, s };
        static readonly Parser<object[]> VARIABLE_CALL = from varname in Varname
                                                         select new object[] { NodeType.VARIABLE_CALL, varname };
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
                                                  select new object[]{ NodeType.LAMBDA, blk };

        static readonly Parser<object[]> BLOCK = from blk in BLOCK_STMT.Token()
                                                 select new object[]{ NodeType.BLOCK, blk };

        static readonly Parser<object[]> HASH = from a in Parse.String("{").Text().Token()
                                                from pairs in Assoc.Many()
                                                from b in Parse.String("}").Text().Token()
                                                select makeHash(pairs.ToArray());
        static readonly Parser<object[]> QUOTE = from m in Parse.String("'").Text()
                                                 from stmt in STMT
                                                 select new object[] { NodeType.QUOTE, toNode(stmt).toSource() };
        static readonly Parser<object[]> QUASI_QUOTE = from m in Parse.String("`").Text()
                                                       from stmt in STMT
                                                       select new object[] { NodeType.QUASI_QUOTE, toNode(stmt).expand() };
        static readonly Parser<object[]> QUESTION_QUOTE = from m in Parse.String("?").Text()
                                                          from stmt in Varname
                                                          select new object[] { NodeType.QUESTION_QUOTE, stmt };

        public static readonly Parser<object[]> PRIMARY = new Parser<object[]>[] { NL, NUMERIC, BOOL, STRING, SYMBOL, ARRAY, HASH, LAMBDA, BLOCK, QUOTE, QUASI_QUOTE, QUESTION_QUOTE }.Aggregate((seed, nxt) => seed.Or(nxt));

        static readonly Parser<object[]> EXP_VAL = (from a in Parse.Char('(').Token()
                                                    from s in STMT
                                                    from b in Parse.Char(')').Token()
                                                    select s).Or(PRIMARY);
        static readonly Parser<object[]> ARGS = from args in Args
                                                select args.Select((arg) => ((object[])STMT.Parse(arg))).ToArray();
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
        static readonly Parser<object[]> STARTER = EXP_VAL.Or(FUNCTION_CALL).Or(VARIABLE_CALL);

        static readonly Parser<object[]> FUNCALL = OperandsChainCallStart(Parse.String("."), STARTER, METHOD_CALL, (dot, op1, op2) => {
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

         static Parser<T> OperandsChainCallStart<T,T2,TOp>(
          Parser<TOp> op,
          Parser<T> operand,
          Parser<T2> operand2,
          Func<TOp, T, T2, T> apply)
        {
            return operand.Then(first => OperandsChainCallRest(first, op, operand, operand2, apply));
        }

        static Parser<T> OperandsChainCallRest<T,T2,TOp>(
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
                (op, a, b) => string.Format("{0}.({1})({2})", a, op, b ));
        }

        static Parser<string> makeLeftUnary(string[] operators, Parser<string> expr)
        {
            return (from h in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                    from v in expr
                    select string.Format("{0}.({1}@)()", v, h )).Or(expr);
        }

        static Parser<string> makeRightUnary(string[] operators, Parser<string> expr)
        {
            return (from v in expr
                    from t in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                    select string.Format("{0}.(@{1})()", v, t )).Or(expr);
        }

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

        static Object.LpObject defClass(string fname, List<Ast.LpAstNode> stmts)
        {
            var o = Object.LpKernel.initialize();
            o.methods[fname] = Object.LpClass.initialize( fname, stmts );
            return (Object.LpObject)o.methods[fname];
        }

        static object[] makeHash( string[][] pairs )
        {
            return new object[]{
                NodeType.HASH,
                pairs.Select((pair) => new object[] { STMT.Parse(pair[0]), STMT.Parse(pair[1]) }).ToArray()
            };
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
            var p = psr.Parse(ctx);
            Console.WriteLine("p");
            Console.WriteLine(p);
            var o = toNode(p);
            Console.WriteLine("o");
            Console.WriteLine(o);
            return o.Evaluate();
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

        static Object.LpObject[] parseArgsObject(string ctx)
        {
            return ARGS.Parse(ctx).Select( (node) => toNode((object[])node).Evaluate() ).ToArray();
        }

        static void benchmarkString(Parser<string> psr, string ctx, int max=1000)
        {
            Console.WriteLine("benckmark:start");
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for( int i=0; i<max; i++ ){
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

        public static Ast.LpAstNode toNode( object[] node ) {
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
                    return toNode( (object[])node[1] );
                case NodeType.STMTS:
                    return Ast.LpAstStmts.toNode( (List<object[]>)node[1] );
                case NodeType.ARRAY:
                    return Ast.LpAstArray.toNode((List<object[]>)node[1]);
                case NodeType.HASH:
                    return Ast.LpAstHash.toNode((object[])node[1]);
                default:
                    return null;
            }
        }
         
        public static Object.LpObject execute(string ctx)
        {
            //Console.WriteLine(ctx);
            var str = Program.Parse(ctx);
            //Console.WriteLine(str);
            var pobj = PROGRAM.Parse(str);
            //Console.WriteLine(pobj);
            var node = toNode(pobj);
            //Console.WriteLine(node);
            var o = node.Evaluate();

            //Console.WriteLine(o.class_name);
            //Console.WriteLine( o.doubleValue);
            return o;
        }
    }
}
