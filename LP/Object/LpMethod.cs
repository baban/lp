using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    // TODO: メソッドの定義位置を記録するべき
    class LpMethod : LpBase
    {
        static string className = "Method";

        public LpMethod(BinMethod m) {
            arguments = new Util.LpArguments();
            method = m;
        }

        public LpMethod(BinMethod m, int max)
        {
            arguments = new Util.LpArguments();
            method = m;
        }

        public static LpObject initialize(BinMethod method)
        {
            var obj = createClassTemplate();
            obj.method = method;
            return obj;
        }

        public static LpObject initialize( string[] args, string[] stmts )
        {
            var obj = createClassTemplate();
            obj.arguments = new Util.LpArguments( args );
            obj.statements = stmts.ToList();
            return obj;
        }

        public static LpObject initialize( Util.LpArguments args, string[] stmts)
        {
            var obj = createClassTemplate();
            obj.arguments = args;
            obj.statements = stmts.ToList();
            return obj;
        }

        private static LpObject createClassTemplate()
        {
            if (classes.ContainsKey(className))
            {
                return classes[className].Clone();
            }
            else
            {
                LpObject obj = new LpObject();
                setMethods(obj);
                obj.superclass = LpObject.initialize();
                obj.class_name = className;
                classes[className] = obj;
                return obj.Clone();
            }
        }

        private static void setMethods(LpObject obj){
            // TODO: comment
            // TODO: arity
            // TODO: inspect
            // TODO: to_s
            // TODO: display
            // TODO: to_block
            // TODO: to_method
            // TODO: to_lambda
            // TODO: call
        }

        public LpObject funcall(LpObject self, LpObject[] args, LpObject block = null)
        {
            return call(self, args, block);
        }

        static public LpObject call(LpObject self, LpObject[] args, LpObject block = null)
        {
            var dstArgs = self.arguments.putVariables(args, block);
            return (self.method != null) ?
                self.method(self, dstArgs) : 
                callString(self, dstArgs, block);
        }

        static private LpObject callString(LpObject self, LpObject[] args, LpObject block = null)
        {
            self.arguments.setVariables(self, args, block);
            LpObject ret = LpNl.initialize();
            self.statements.ForEach(delegate(string stmt)
            {
                ret = LpParser.STMT.Parse(stmt);
            });
            return ret;
        }

    }

}
