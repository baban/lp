using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpQuasiQuote : LpBase
    {
        static string className = "QuasiQuote";

        public static LpObject initialize()
        {
            return init("");
        }

        public static LpObject initialize(string s)
        {
            return init(s);
        }

        private static LpObject init(string s)
        {
            LpObject obj = createClassTemplate();
            obj.stringValue = s;
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

        private static void setMethods(LpObject obj)
        {
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
        }

        private static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(self.stringValue);
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine(self.stringValue);
            return LpNl.initialize();
        }
    }
}
