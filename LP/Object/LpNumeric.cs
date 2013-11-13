using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpNumeric : LpObject
    {
        public static LpObject initialize()
        {
            return init(0);
        }

        public static LpObject initialize(double i)
        {
            return init(i);
        }

        public static LpObject initialize(int i)
        {
            return init((double)i);
        }

        public static LpObject initialize(string s)
        {
            Console.WriteLine("numeric#init");
            Console.WriteLine(s);
            return init( double.Parse(s) );
        }


        private static LpObject init(double i)
        {
            LpObject obj = LpObject.initialize();
            obj.class_name = "numeric";
            obj.superclass = LpObject.initialize();
            setMethods(obj);
            obj.doubleValue = i;
            return obj;
        }

        private static void setMethods( LpObject obj )
        {
            //methods["+"] = new MethodObj(plus);
            //methods["-"] = new MethodObj(minus);
            //methods["/"] = new MethodObj(times);
            //methods["*"] = new MethodObj(div);
            //methods["%"] = new MethodObj(mod);
            //methods[">"] = new MethodObj(compareTo);
            //methods["**"] = new MethodObj(pow);
            //methods["=="] = new MethodObj(equal);
            //methods["==="] = new MethodObj(eq);

            obj.methods["to_s"] = new BinMethod(to_s);
        }


        protected static LpObject display(LpObject self, LpObject args)
        {
            Console.WriteLine(self.doubleValue.ToString());
            return null;
        }

        protected static LpObject to_s(LpObject self, LpObject args)
        {
            return LpString.initialize(self.doubleValue.ToString());
        }

        protected static LpObject inspect(LpObject self, LpObject args)
        {
            return LpString.initialize(self.doubleValue.ToString());
        }
    }
}
