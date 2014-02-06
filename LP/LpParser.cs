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
    class LpParser
    {
        // Expressions
        static readonly string[][] operandTable = new string[][] {
            new string[]{ "**" },
            new string[]{ "*","/", "%" },
            new string[]{ "+","-" },
            new string[]{ "<<",">>" },
            new string[]{ "&" },
            new string[]{ "|" },
            new string[]{ ">=", ">", "<=", "<" },
            new string[]{ "<=>", "===", "==", "!=", "=~", "!~" },
            new string[]{ "&&" },
            new string[]{ "||" },
            new string[]{ "..", "^..", "..^", "^..^" },
            new string[]{ "and", "or" }
        };

        // 基本文字一覧
        static readonly Parser<string> Term = Parse.Regex("^[;\n]");

        // Primary Values
        static readonly Parser<string> Identifier = Parse.Regex("[a-zA-Z_][a-zA-Z0-9_]*");

        static readonly Parser<string> Bool = Parse.Regex("true|false").Text().Token();
        static readonly Parser<string> Decimal = Parse.Regex(@"\d+\.\d+").Text().Token();
        static readonly Parser<string> Int = Parse.Regex(@"\d+").Text().Token();
        static readonly Parser<string> Numeric = Decimal.Or(Int).Token();
        static readonly Parser<string> Symbol = from mark in Parse.String(":").Text()
                                                from idf in Identifier
                                                select mark+idf;
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

        static readonly Parser<string> OperandMarks = new string[] { "**", "*", "/", "%", "+", "-", "<<", ">>", "&", "|", ">=", ">", "<=", "<", "<=>", "===", "==", "!=", "=~", "!~", "&&", "||", "and", "or" }.Select(op => Parse.String(op)).Aggregate((op1, op2) => op1.Or(op2)).Text();

        static readonly Parser<string> Fname = (from a in Parse.String("(").Text()
                                                from v in OperandMarks.Or(Identifier)
                                                from b in Parse.String(")").Text()
                                                select a + v + b).Or(Identifier);

        static readonly Parser<string> FCallname = (from a in Parse.String("(").Text()
                                                    from v in OperandMarks.Or(Identifier)
                                                    from b in Parse.String(")").Text()
                                                    select v).Or(Identifier);

        // Complex Data Values
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

        static readonly Parser<string[]> BlaketArgs = from a in Parse.Char('(')
                                                      from args in Parse.Ref(() => ArgList)
                                                      from b in Parse.Char(')')
                                                      select args;
        static readonly Parser<string[]> FenceArgs = from a in Parse.Char('|')
                                                     from args in Parse.Ref(() => ArgList)
                                                     from b in Parse.Char('|')
                                                     select args;

        static readonly Parser<string> BlockStart1 = Parse.String("do").Token().Text();
        static readonly Parser<string> BlockStart2 = from args in BlaketArgs.Token()
                                                     from _do in Parse.String("do").Token()
                                                     select "(" + string.Join(", ", args) + ") do";
        static readonly Parser<string> BlockStart3 = from _do in Parse.String("do").Token()
                                                     from args in FenceArgs.Token()
                                                     select "do |" + string.Join(", ", args) + "|";

        static readonly Parser<string> Block = from start in BlockStart3.Or(BlockStart2).Or(BlockStart1)
                                               from stmts in Parse.Ref(() => Stmts)
                                               from c in Parse.String("end").Token()
                                               select start + " " + string.Join("; ", stmts.ToArray()) + " end";

        static readonly Parser<string> Lambda = from a in Parse.String("^do").Token()
                                                from stmts in Parse.Ref(() => Stmts)
                                                from c in Parse.String("end").Token()
                                                select "^do " + string.Join("; ", stmts.ToArray()) + " end";

        static readonly Parser<string> Function = from a in Parse.String("def").Token()
                                                  from fname in Identifier
                                                  from args in ArgDecl
                                                  from b in Term
                                                  from stmts in Stmts
                                                  from c in Parse.String("end")
                                                  select "def " + fname + "(" + string.Join(", ", args.ToArray()) + ");" + 
                                                         string.Join( "; ", stmts.ToArray()) + 
                                                         " end";

        static readonly Parser<string> Funcall = (from idf in Fname
                                                       from c in Parse.Char('(').Once().Text()
                                                       from args in Args
                                                       from d in Parse.Char(')').Once().Text()
                                                       select idf + c + string.Join(", ", args.ToArray()) + d).Or(
                                                       from idf in Fname
                                                       select idf+"()");

        static readonly Parser<string> FCallnameCall = (from idf in FCallname
                                                        from c in Parse.Char('(').Once().Text()
                                                        from args in Args
                                                        from d in Parse.Char(')').Once().Text()
                                                        select idf + c + string.Join(", ", args.ToArray()) + d).Or(
                                                        from idf in FCallname
                                                        select idf + "()");

        static readonly Parser<string> Primary = new Parser<string>[] { Numeric, Bool, String, Symbol, Array, Hash, Lambda, Block, Comment }.Aggregate((seed, nxt) => seed.Or(nxt));

        static readonly Parser<string> ExpVal = (from a in Parse.Char('(').Token()
                                                 from v in Expr
                                                 from b in Parse.Char(')').Token()
                                                 select a + v + b).Or(Primary);
        // ::
        static readonly Parser<string> Classcall = OperandsChainCallStart(Parse.String("::"), ExpVal, Funcall, (opr, a, b) => a + opr + b);
        static readonly Parser<string> ExpClasscall = Classcall.Or(ExpVal);

        // .
        static readonly Parser<string> MethodCall = OperandsChainCallStart(Parse.Char('.').Once().Text(), ExpClasscall, Funcall, (opr, a, b) => a + opr + b);
        static readonly Parser<string> ExpFuncall = MethodCall.Or(ExpClasscall);
        // []
        static readonly Parser<string> ExpArrayAt = ExpFuncall;
        // ++, --
        static readonly Parser<string> ExpUnary = ExpArrayAt;
        // +(単項)  !  ~
        static readonly Parser<string> ExpUnaryPlus = ExpUnary;
        // not
        static readonly Parser<string> ExpNot = ExpUnaryPlus;
        // **
        static readonly Parser<string> ExpSquare = makeExpr(operandTable[0], ExpNot);
        // -(単項)
        static readonly Parser<string> ExpUnaryMinus = ExpSquare;
        // *, /
        static readonly Parser<string> ExpMul = makeExpr(operandTable[1], ExpUnaryMinus);
        // +,-
        static readonly Parser<string> ExpAdditive = makeExpr(operandTable[2], ExpMul);
        // << >>
        static readonly Parser<string> ExpShift = makeExpr(operandTable[3], ExpAdditive);
        // & 
        static readonly Parser<string> ExpAnd = makeExpr(operandTable[4], ExpShift);
        // |
        static readonly Parser<string> ExpInclusiveOr = makeExpr(operandTable[5], ExpAnd);
        // ^ 
        static readonly Parser<string> ExpRelational = ExpInclusiveOr;
        // > >=  < <= 
        static readonly Parser<string> ExpExclusiveOr = makeExpr(operandTable[6], ExpRelational);
        // <=> ==  === !=  =~  !~ 
        static readonly Parser<string> ExpEquality = makeExpr(operandTable[7], ExpExclusiveOr);
        // &&
        static readonly Parser<string> ExpLogicalAnd = makeExpr(operandTable[8], ExpEquality);
        // ||
        static readonly Parser<string> ExpLogicalOr = makeExpr(operandTable[9], ExpLogicalAnd);
        // ..
        static readonly Parser<string> ExpRange = makeExpr(operandTable[10], ExpLogicalOr);
        // =(+=, -= ... )
        static readonly Parser<string> ExpAssignment = ExpRange;
        // and or
        static readonly Parser<string> ExpAndOr = makeExpr(operandTable[11], ExpAssignment);
        // =
        static readonly Parser<string> ExpEqual = Parse.ChainOperator(
            Operator("="),
            ExpAndOr,
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
                                               select s).Or(StatList);

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
                                                        select elms.Aggregate(Object.LpArray.initialize(), (args, s) => args.funcall("push", STMT.Parse(s)));

        static readonly Parser<Object.LpObject> HASH = from a in Parse.String("{").Text().Token()
                                                       from pairs in Assoc.Many()
                                                       from b in Parse.String("}").Text().Token()
                                                       select makeHash(pairs.ToArray() );

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

        static readonly Parser<Object.LpObject> ARGS = from gs in Args
                                                       select gs.ToArray().Aggregate(Object.LpArguments.initialize(), (args, s) => { args.funcall("push", STMT.Parse(s)); return args; });

        static readonly Parser<Object.LpObject> ARGS_CALL = (from a in Parse.Char('(')
                                                             from args in ARGS
                                                             from b in Parse.Char(')')
                                                             select args).Or(ARGS);

        static readonly Parser<Object.LpObject> FUNCTION = from a in Parse.String("def").Token()
                                                           from fname in Fname.Text()
                                                           from args in ArgDecl
                                                           from stmts in Stmts
                                                           from c in Parse.String("end").Token()
                                                           select def_function(fname, args, stmts);

        static readonly Parser<Object.LpObject> FUNCTION_CALL = from fname in Fname
                                                                from args in ARGS_CALL
                                                                select Object.LpIndexer.last().funcall( fname, args );

        static readonly Parser<Object.LpObject> FUNCALL = OperandsChainCallRestVStart( Parse.Ref( () => EXP_VAL ) );

        static readonly Parser<Object.LpObject> DEF_CLASS = from a in Parse.String("class").Token()
                                                            from fname in Identifier.Text()
                                                            from b in Term
                                                            from stmts in Stmts
                                                            from c in Parse.String("end").Token()
                                                            select def_class(fname, stmts);

        static readonly Parser<Object.LpObject> EXP_VAL = (from a in Parse.Char('(').Token()
                                                           from v in STMT
                                                           from b in Parse.Char(')').Token()
                                                           select v).Or(PRIMARY);

        static readonly Parser<Object.LpObject> EXPR = FUNCALL.Or(FUNCTION_CALL).Or(EXP_VAL);
        public static readonly Parser<Object.LpObject> STMT = EXPR;

        static readonly Parser<Object.LpObject> PROGRAM = from stmts in Stmts
                                                          select stmts.ToArray().Aggregate(Object.LpNl.initialize(), ( ret , s) => { ret = STMT.Parse(s); return ret; });

        static Parser<T> OperandsChainCallStart<T,T2, TOp>(
          Parser<TOp> op,
          Parser<T> operand,
          Parser<T2> operand2,
          Func<TOp, T, T2, T> apply)
        {
            return operand.Then(first => OperandsChainCallRest(first, op, operand, operand2, apply));
        }

        static Parser<T> OperandsChainCallRest<T, T2,TOp>(
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

        static Parser<Object.LpObject> OperandsChainCallRestVStart(Parser<Object.LpObject> operand)
        {
            Parser<string> op = Parse.Char('.').Once().Text();
            Parser<string> fname_op = FCallname;
            Parser<Object.LpObject> args_op = ARGS_CALL;
            Func<Object.LpObject, string, Object.LpObject, Object.LpObject> apply = (obj, fname, args) => obj.funcall(fname, args);
            return operand.Then(first => OperandsChainCallRestV(first));
        }

        static Parser<Object.LpObject> OperandsChainCallRestV(Object.LpObject firstOperand)
        {
            Parser<string> op = Parse.Char('.').Once().Text();
            Parser<string> fname_op = FCallname;
            Parser<Object.LpObject> args_op = ARGS_CALL;
            Func<Object.LpObject, string, Object.LpObject, Object.LpObject> apply = (obj, fname, args) => obj.funcall(fname, args);
            return Parse.Or(op.Then(opvalue =>
                            fname_op.Then(fname =>
                            args_op.Then(args => OperandsChainCallRestV(apply(firstOperand, fname, args))))),
                      Parse.Return(firstOperand));
        }

        static Parser<string> makeExpr(string[] operators, Parser<string> beforeExpr)
        {
            return Parse.ChainOperator(
                operators.Select(op => Operator(op)).Aggregate((op1, op2) => op1.Or(op2)),
                beforeExpr,
                (op, a, b) => a + ".(" + op + ")(" + b + ")");
        }

        static Parser<string> Operator(string operand)
        {
            return Parse.String(operand).Token().Text();
        }

        static Object.LpObject def_function(string fname, string[] args, string[] stmts)
        {
            var o = Object.LpKernel.initialize();
            o.methods[fname] = Object.LpMethod.initialize(args, stmts);
            return null;
        }

        static Object.LpObject def_class(string fname, string[] stmts)
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
