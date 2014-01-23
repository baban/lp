using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpKernel : LpBase
    {
        static string className = "Kernel";

        public static LpObject initialize()
        {
            return createClassTemplate();
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
                obj.superclass = null;
                obj.class_name = className;
                classes[className] = obj;
                return obj;
            }
        }

        static private LpObject setMethods(LpObject obj)
        {
            // TODO: yield
            // TODO: return
            // TODO: super
            // TODO: while
            // TODO: until
            // TODO: until
            // TODO: alias
            // TODO: break
            // TODO: next
            // TODO: retry

            // 構文
            // TODO: self

            obj.methods["print"] = new LpMethod( new BinMethod(print) );
            obj.methods["if"] = new LpMethod( new BinMethod(_if) );
            return obj;
        }

        private static LpObject print(LpObject self, LpObject args)
        {
            var o = args.arrayValues.First();
            o.funcall("display",null);
            return null;
        }

        private static LpObject _if(LpObject self, LpObject args)
        {
            var expr = args.arrayValues.ElementAt(0);
            Func<LpObject, LpObject> fun = (stmt) => (stmt.class_name == "Block" || stmt.class_name == "Lambda") ? stmt.funcall("call", null) : stmt;
            if( expr != null && expr.boolValue != false ){
                return fun( args.arrayValues.ElementAt(1) );
            } else {
                return fun( args.arrayValues.ElementAt(2) );
            }
        }
    }
}
