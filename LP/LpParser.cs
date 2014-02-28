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
    // TODO: nil
    // Block読み出し
    // TODO: インスタンス変数
    // TODO: グローバル変数
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

        // Primary Values
        static readonly Parser<string> Identifier = Parse.Regex("[a-zA-Z_][a-zA-Z0-9_]*");

        static readonly Parser<string> Bool = Parse.Regex("true|false").Text();
        static readonly Parser<string> Decimal = Parse.Regex(@"\d+\.\d+").Text();
        static readonly Parser<string> Int = Parse.Regex(@"\d+").Text();
        static readonly Parser<string> Numeric = Decimal.Or(Int);
        static readonly Parser<string> Symbol = from mark in Parse.String(":").Text()
                                                from idf in Identifier
                                                select mark+idf;
        // TODO: 変数展開実装
        static readonly Parser<string> String = from a in Parse.Char('"')
                                                from s in ( Parse.Char('\\').Once().Concat( Parse.Char('"').Once() )).Or(Parse.CharExcept('"').Once()).Text().Many()
                                                from b in Parse.Char('"')
                                                select '"' + string.Join("", s.ToArray() ) + '"';
        // Comment
        static readonly Parser<string> InlineComment = from a in Parse.Regex("//.*\n")
                                                       select "";
        static readonly Parser<string> BlockComment = from a in Parse.Regex(@"/\*.*?\*/")
                                                      select "";

        static readonly Parser<string> Comment = InlineComment.Or(BlockComment);

        static readonly Parser<string> Fname = (from a in Parse.String("(").Text()
                                                from v in OperandMarks.Or(Identifier)
                                                from b in Parse.String(")").Text()
                                                select a + v + b).Or(Identifier);

        static readonly Parser<string> Varname = Identifier;

        // Array, Hash
        static readonly Parser<string> SepElm = (from sep in Parse.Char(',').Token()
                                                 from s in Parse.Ref(() => Stmt)
                                                 select s).Or(Parse.Ref(() => Stmt));

        static readonly Parser<string> Array = from a in Parse.String("[").Text().Token()
                                               from elms in SepElm.Many()
                                               from b in Parse.String("]").Text().Token()
                                               select a + string.Join(",", elms) + b;

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

        // Block,Lambda
        static readonly Parser<string[]> BlaketArgs = from a in Parse.Char('(')
                                                      from args in Parse.Ref(() => ArgList)
                                                      from b in Parse.Char(')')
                                                      select args;
        static readonly Parser<string[]> FenceArgs = from a in Parse.Char('|')
                                                     from args in Parse.Ref(() => ArgList)
                                                     from b in Parse.Char('|')
                                                     select args;

        static readonly Parser<string> BlockStart1 = from _do in Parse.String("do").Text()
                                                     from ws in Parse.WhiteSpace.Many()
                                                     select _do;
        static readonly Parser<string> BlockStart2 = from args in BlaketArgs
                                                     from _do in Parse.String("do").Token()
                                                     select "(" + string.Join(", ", args) + ") do";
        static readonly Parser<string> BlockStart3 = from _do in Parse.String("do")
                                                     from ws in Parse.WhiteSpace.Many()
                                                     from args in FenceArgs.Token()
                                                     select "do |" + string.Join(", ", args) + "|";

        static readonly Parser<string> Block = from start in BlockStart3.Or(BlockStart2).Or(BlockStart1)
                                               from stmts in Parse.Ref(() => Stmts)
                                               from c in Parse.String("end").Token()
                                               select start + " " + string.Join("; ", stmts.ToArray()) + " end";

        static readonly Parser<string> Lambda = from h in Parse.String("^").Text()
                                                from blk in Block
                                                select h+blk;

        static readonly Parser<string> Function = from a in Parse.String("def").Token()
                                                  from fname in Identifier
                                                  from args in ArgDecl
                                                  from b in Term
                                                  from stmts in Stmts
                                                  from c in Parse.String("end")
                                                  select "def " + fname + "(" + string.Join(", ", args.ToArray()) + ");" + 
                                                         string.Join( "; ", stmts.ToArray()) + 
                                                         " end";

        static readonly Parser<string> Funcall0 = from idf in Fname
                                                  select idf + "()";
        static readonly Parser<string> FuncallArg = from idf in Fname
                                                    from c in Parse.Char('(').Once().Text()
                                                    from args in Args
                                                    from d in Parse.Char(')').Once().Text()
                                                    select idf + "(" + string.Join(", ", args.ToArray()) + ")";

        static readonly Parser<string> FuncallBlk = from fcall in FuncallArg
                                                    from blk in Block.Token()
                                                    select fcall+" "+blk;

        static readonly Parser<string> Varcall = Varname;

        static readonly Parser<string> Funcall = FuncallBlk.Or(FuncallArg);

        static readonly Parser<string> Primary = new Parser<string>[] { Numeric, Bool, String, Symbol, Array, Hash, Lambda, Block, Comment }.Aggregate((seed, nxt) => seed.Or(nxt)).Token();

        static readonly Parser<string> ExpVal = (from a in Parse.Char('(').Token()
                                                 from v in Expr
                                                 from b in Parse.Char(')').Token()
                                                 select a + v + b).Or(Primary).Or(Funcall);
        // ::
        static readonly Parser<string> Classcall = OperandsChainCallStart(Parse.String("::"), ExpVal, Funcall.Or(Funcall0), (opr, a, b) => a + opr + b);
        static readonly Parser<string> ExpClasscall = Classcall.Or(ExpVal);

        // .
        static readonly Parser<string> MethodCall = OperandsChainCallStart(Parse.String(".").Text(), ExpClasscall, Funcall.Or(Funcall0), (opr, a, b) => a + opr + b);
        static readonly Parser<string> ExpMethodcall = MethodCall.Or(ExpClasscall);
        // []
        static readonly Parser<string> ExpArrayAt = (from expr in ExpMethodcall
                                                     from a in Parse.Char('[')
                                                     from v in Stmt
                                                     from b in Parse.Char(']')
                                                     select expr + ".([])(" + v + ")").Or(ExpMethodcall);
        // ２項演算子一覧
        static readonly Parser<string> ChainExprs = makeExpressions(operandTable, ExpArrayAt);
        // =(+=, -= ... )
        // TODO: 代入演算子作成
        static readonly Parser<string> ExpAssignment = ChainExprs;
        // =
        // TODO: デバッグ足りていない
        static readonly Parser<string> ExpEqual = Parse.ChainOperator(
            Operator("="),
            ExpAssignment,
            (op, a, b) => a + ".(" + op + ")(" + b + ")");

        // 演算子一覧
        static readonly Parser<string> Expr = ExpEqual;

        // arguments
        static readonly Parser<string> Arg = Parse.Ref(() => Stmt);
        static readonly Parser<string[]> ZeroArgs = from sps in Parse.WhiteSpace.Many()
                                                    select new string[]{};
        static readonly Parser<string[]> Args = from ags in Parse.DelimitedBy(Arg, Parse.Char(',').Token()).Or(ZeroArgs)
                                                select ags.ToArray();

        static readonly Parser<string> AstArg = from ast in Parse.Char('*').Once()
                                                from id in Identifier
                                                select ast + id;
        static readonly Parser<string> AmpArg = from ast in Parse.Char('&').Once()
                                                from id in Identifier
                                                select ast + id;
        static readonly Parser<IEnumerable<string>> SimpleArgList = Parse.DelimitedBy(Identifier, Parse.Char(',').Token());
        static readonly Parser<string[]> ArgList = from ags in
                                                       (from args in SimpleArgList
                                                        from astarg in AstArg
                                                        from amparg in AmpArg
                                                        select args.Concat(new string[] { astarg, amparg })).Or(
                                                        from args in SimpleArgList
                                                        from amparg in AmpArg
                                                        select args.Concat(new string[] { amparg })).Or(
                                                        from args in SimpleArgList
                                                        from astarg in AstArg
                                                        select args.Concat(new string[] { astarg })).Or(SimpleArgList).Or(ZeroArgs)
                                                   select ags.ToArray();
        static readonly Parser<string[]> ArgDecl = from a in Parse.Char('(').Once().Text()
                                                   from args in ArgList
                                                   from b in Parse.Char(')').Once().Text()
                                                   select args;

        // Macro Values
        static readonly Parser<string> Quote = from qmark in Parse.String("'").Text()
                                               from idf in Stmt
                                               select idf;

        static readonly Parser<string> QuasiQuote = from qmark in Parse.String("`").Text()
                                                    from idf in Stmt
                                                    select idf;

        // is Statements
        static readonly Parser<string> IfExpr =  Parse.Ref( () => (from a in Parse.Char('(')
                                                                   from stmt in Stmt
                                                                   from b in Parse.Char(')')
                                                                   select stmt).Or(
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
        static readonly Parser<string> IfStmt1 = from expr in IfStart
                                                 from stmts1 in Stmts
                                                 from c in IfEnd
                                                 select "if(" + expr + ",do " + string.Join("; ", stmts1.ToArray()) + " end)";
        static readonly Parser<string> IfStmt2 = from expr in IfStart
                                                 from stmts1 in Stmts
                                                 from stmts2 in ElseStmts
                                                 from c in IfEnd
                                                 select "if(" + expr + ",do " + string.Join("; ", stmts1.ToArray()) + " end,do " + string.Join("; ", stmts2.ToArray()) + " end)";
        static readonly Parser<string> IfStmt = IfStmt2.Or(IfStmt1);

        static readonly Parser<string> StatCollection = Function.Or(IfStmt);
        static readonly Parser<string> StatList = StatCollection.Or(Expr);

        static readonly Parser<string> Stmt = (from s in StatList
                                               from t in Term
                                               select s).Or(StatList).Token();

        static readonly Parser<string[]> Stmts = from stmts in Stmt.Many()
                                                 select stmts.ToArray();

        static readonly Parser<string> Program = from stmts in Stmts
                                                 select string.Join( "; ", stmts.ToArray() );

        // Primary Values
        static readonly Parser<Object.LpObject> INT = from n in Int
                                                      select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> NUMERIC = from n in Numeric
                                                          select Object.LpNumeric.initialize(double.Parse(n));

        static readonly Parser<Object.LpObject> BOOL = from b in Bool
                                                       select Object.LpBool.initialize( bool.Parse(b) );
        // TODO: 変数展開を入れる
        static readonly Parser<Object.LpObject> STRING = from a in Parse.Char('"')
                                                         from s in Parse.CharExcept('"').Many().Text()
                                                         from b in Parse.Char('"')
                                                         select Object.LpString.initialize(s);

        static readonly Parser<Object.LpObject> SYMBOL = from m in Parse.String(":").Text()
                                                         from s in Identifier
                                                         select Object.LpSymbol.initialize(s);

        static readonly Parser<Object.LpObject> ARRAY = from a in Parse.String("[").Text().Token()
                                                        from elms in SepElm.Many()
                                                        from b in Parse.String("]").Text().Token()
                                                        select Object.LpArray.initialize( elms.ToArray() );

        static readonly Parser<Object.LpObject> HASH = from a in Parse.String("{").Text().Token()
                                                       from pairs in Assoc.Many()
                                                       from b in Parse.String("}").Text().Token()
                                                       select makeHash( pairs.ToArray() );

        static readonly Parser<object[]> BLOCK_START1 = from a in Parse.String("do").Token()
                                                        select new object[]{ new string[]{}, false };
        static readonly Parser<object[]> BLOCK_START2 = from _do in Parse.String("do").Token()
                                                        from args in FenceArgs.Token()
                                                        select new object[] { args, false };
        static readonly Parser<object[]> BLOCK_START3 = from args in BlaketArgs.Token()
                                                        from _do in Parse.String("do").Token()
                                                        select new object[] { args, true };

        static readonly Parser<Object.LpObject> BLOCK = from args in BLOCK_START3.Or(BLOCK_START2).Or(BLOCK_START1)
                                                        from stmts in Stmts
                                                        from b in Parse.String("end").Token()
                                                        select Object.LpBlock.initialize( stmts, args );

        static readonly Parser<Object.LpObject> LAMBDA = from head in Parse.Char('^')
                                                         from args in BLOCK_START3.Or(BLOCK_START2).Or(BLOCK_START1)
                                                         from stmts in Stmts
                                                         from b in Parse.String("end").Token()
                                                         select Object.LpLambda.initialize( stmts, args );

        public static readonly Parser<Object.LpObject> PRIMARY = new Parser<Object.LpObject>[] { NUMERIC, BOOL, STRING, SYMBOL, ARRAY, HASH, BLOCK, LAMBDA }.Aggregate((seed, nxt) => seed.Or(nxt));

        static readonly Parser<Object.LpObject[]> ARGS = from gs in Args
                                                         select gs.Select((s) => STMT.Parse(s) ).ToArray();

        static readonly Parser<Object.LpObject[]> ARGS_CALL = from a in Parse.Char('(')
                                                              from args in ARGS
                                                              from b in Parse.Char(')')
                                                              select args;

        static readonly Parser<Object.LpObject> FUNCTION = from a in Parse.String("def").Token()
                                                           from fname in Fname.Text()
                                                           from args in ArgDecl
                                                           from stmts in Stmts
                                                           from c in Parse.String("end").Token()
                                                           select defFunction(fname, args, stmts);

        static readonly Parser<Object.LpObject> VARIABLE_CALL = from varname in Varname
                                                                select Util.LpIndexer.last().varcall(varname);

        static readonly Parser<object[]> METHOD_CALL = (from fname in Fname
                                                        from args in ARGS_CALL
                                                        from blk in BLOCK
                                                        select new object[] { (string)fname, (Object.LpObject[])args, blk }).Or(
                                                        from fname in Fname
                                                        from args in ARGS_CALL
                                                        select new object[] { (string)fname, (Object.LpObject[])args, null });

        static readonly Parser<Object.LpObject> FUNCTION_CALL = (from fvals in METHOD_CALL
                                                                select Util.LpIndexer.last().funcall((string)fvals[0], (Object.LpObject[])fvals[1], (Object.LpObject)fvals[2])).Or(VARIABLE_CALL);

        static readonly Parser<Object.LpObject> FUNCALL = OperandsChainCallStart(Parse.String(".").Text(), Parse.Ref(() => EXP_VAL), METHOD_CALL, (opr, obj, fvals) => obj.funcall((string)fvals[0], (Object.LpObject[])fvals[1]));

        static readonly Parser<Object.LpObject> DEF_CLASS = from a in Parse.String("class").Token()
                                                            from fname in Identifier.Text()
                                                            from b in Term
                                                            from stmts in Stmts
                                                            from c in Parse.String("end").Token()
                                                            select defClass(fname, stmts);

        static readonly Parser<Object.LpObject> EXP_VAL = (from a in Parse.Char('(').Token()
                                                           from v in STMT
                                                           from b in Parse.Char(')').Token()
                                                           select v).Or(PRIMARY);

        static readonly Parser<Object.LpObject> EXPR = new Parser<Object.LpObject>[] { FUNCALL, FUNCTION_CALL, VARIABLE_CALL, EXP_VAL }.Aggregate((seed, nxt) => seed.Or(nxt)).Token();

        public static readonly Parser<Object.LpObject> STMT = EXPR;

        public static readonly Parser<Object.LpObject> PROGRAM = from stmts in Stmts
                                                                 select stmts.ToArray().Aggregate(Object.LpNl.initialize(), (ret, s) => ret = STMT.Parse(s) );

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
                (op, a, b) => a + ".(" + op + ")(" + b + ")");
        }

        static Parser<string> makeLeftUnary(string[] operators, Parser<string> expr)
        {
            return (from h in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                    from v in expr
                    select v+".("+h+"@)()").Or(expr);
        }

        static Parser<string> makeRightUnary(string[] operators, Parser<string> expr)
        {
            return (from v in expr
                    from t in operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2))
                    select v + ".(@" + t + ")()").Or(expr);
        }

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

        static Object.LpObject defFunction(string fname, string[] args, string[] stmts)
        {
            var o = Object.LpKernel.initialize();
            o.methods[fname] = Object.LpMethod.initialize(args, stmts);
            return null;
        }

        static Object.LpObject defClass(string fname, string[] stmts)
        {
            var o = Object.LpKernel.initialize();
            o.methods[fname] = Object.LpClass.initialize( fname, stmts );
            return (Object.LpObject)o.methods[fname];
        }

        static Object.LpObject makeHash( string[][] pairs )
        {
            Object.LpObject hash = Object.LpHash.initialize();
            return hash;
        }

        static Object.LpObject doStmts(string[] stmts)
        {
            Object.LpObject ret=null;
            foreach (var stmt in stmts)
            {
                ret = STMT.Parse(stmt);
            }
            return ret;
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

        static Object.LpObject[] parseArgsObject(string ctx)
        {
            Console.WriteLine(ctx);
            return ARGS.Parse(ctx);
        }

        public static Object.LpObject execute(string ctx)
        {
            Parser<string> expander = Program;
            var expanded_code = expander.Parse(ctx);
            Console.WriteLine("Expanded Code:");
            Console.WriteLine(expanded_code);
            var parser = PROGRAM;
            Console.WriteLine("Exec Program:");
            return parser.Parse(expanded_code);
        }
    }
}
