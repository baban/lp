using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpNl : LpBase
    {
        static string className = "Nl";

        public static LpObject initialize()
        {
            return init();
        }

        private static LpObject init()
        {
            LpObject obj = createClassTemplate();
            obj.class_name = className;

            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            /*
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["size"] = new BinMethod(size);

            obj.methods["<<"] = new BinMethod(add);
            obj.methods["+"] = new BinMethod(plus);

            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);
             */
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
