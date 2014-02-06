using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Error
{
    class LpError : Exception
    {
        static string className = "Error";
        /*
        public static Object.LpObject initialize()
        {
            return init();
        }

        private static Object.LpObject Object.init()
        {
            Object.LpObject obj = createClassTemplate();
            return obj;
        }

        private static void setMethods(Object.LpObject obj)
        {
            //obj.methods["to_s"] = new BinMethod(to_s);
        }

        private static Object.LpObject createClassTemplate()
        {
            if (LpBase.classes.ContainsKey(className))
            {
                return LpBase.classes[className].Clone();
            }
            else
            {
                LpObject obj = new LpObject();
                setMethods(obj);
                obj.superclass = LpObject.initialize();
                obj.class_name = className;
                LpBase.classes[className] = obj;
                return obj.Clone();
            }
        }
        */
    }
}
