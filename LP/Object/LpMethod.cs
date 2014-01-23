using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpMethod : LpBase
    {
        static string className = "Method";

        public LpMethod(BinMethod m)
        {
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

        private static LpObject setMethods(LpObject obj){
            return obj;
        }

        public LpObject funcall(LpObject self, LpObject args)
        {
            if ( null != method ){
                return method(self, args);
            }
            else
            {
                return execute( self, args );
            }
            // TODO: display
            // TODO: inspect
            // TODO: to_s
        }

        static LpObject execute(LpObject self, LpObject args)
        {
            LpObject ret = null;
            self.statements.ForEach(delegate(string stmt)
            {
                ret = LpParser.STMT.Parse(stmt);
            });
            return ret;
        }
    }
}
