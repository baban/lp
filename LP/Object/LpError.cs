using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpError : LpBase
    {
        static string className = "Error";

        public static LpObject initialize()
        {
            return init();
        }

        private static LpObject init()
        {
            LpObject obj = createClassTemplate();
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            //obj.methods["to_s"] = new BinMethod(to_s);
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
    }
}
