using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // TODO: display
            // TODO: inspect
            // TODO: to_s
            return method(self, args);
        }
        
    }
}
