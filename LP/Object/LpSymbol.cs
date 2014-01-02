using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpSymbol : LpObject
    {
        public static LpObject initialize(string s)
        {
            return init(s);
        }

        private static LpObject init(string s)
        {
            LpObject obj = LpObject.initialize();
            setMethods(obj);
            obj.superclass = LpObject.initialize();
            obj.stringValue = s;
            obj.class_name = "symbol";
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            // TODO: ==
            // TODO: ===
            // TODO: display
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);

            //obj.methods["="] = new BinMethod(setOp);
            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);
        }

        protected static LpObject to_s(LpObject self, LpObject args)
        {
            return self;
        }

        protected static LpObject inspect(LpObject self, LpObject args)
        {
            return self;
        }

        protected static LpObject display(LpObject self, LpObject args)
        {
            Console.WriteLine(self.stringValue);
            return null;
        }

        private static LpObject equal(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.stringValue == o.stringValue);
        }

        // TODO: This method must comare to memory address!!
        private static LpObject eq(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.stringValue == o.stringValue);
        }
    }
}
