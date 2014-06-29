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
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["display"] = new BinMethod(display);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["inspect"] = new BinMethod(inspect);
            /*
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

        protected static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize("");
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = to_s(self, args);
            Console.WriteLine(v.stringValue);
            return LpNl.initialize();
        }

        protected static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize("nl");
        }
    }
}
