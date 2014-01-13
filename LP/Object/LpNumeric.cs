using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpNumeric : LpBase
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
            return init( double.Parse(s) );
        }

        private static LpObject init(double i)
        {
            LpObject obj = LpObject.initialize();
            obj.class_name = "Numeric";
            obj.superclass = LpObject.initialize();
            setMethods(obj);
            obj.doubleValue = i;
            return obj;
        }

        private static void setMethods( LpObject obj )
        {
            obj.methods["+"]  = new BinMethod(plus);
            obj.methods["-"]  = new BinMethod(minus);
            obj.methods["*"]  = new BinMethod(mul);
            obj.methods["/"]  = new BinMethod(div);
            obj.methods["%"]  = new BinMethod(mod);
            obj.methods["**"] = new BinMethod(pow);

            obj.methods[">"]        = new BinMethod(compareToGreater);
            obj.methods[">="]       = new BinMethod(compareToGreaterEqual);
            obj.methods["<"]        = new BinMethod(compareToLower);
            obj.methods["<="]       = new BinMethod(compareToLowerEqual);
            obj.methods["=="]       = new BinMethod(equal);
            obj.methods["==="]      = new BinMethod(eq);
            obj.methods["between?"] = new BinMethod(between);

            obj.methods["to_i"] = new BinMethod(to_i);
            obj.methods["to_f"] = new BinMethod(to_f);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
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

        protected static LpObject to_f(LpObject self, LpObject args)
        {
            self.doubleValue = self.doubleValue;
            return self;
        }

        protected static LpObject to_i(LpObject self, LpObject args)
        {
            self.doubleValue = (int)self.doubleValue;
            return self;
        }

        private static LpObject plus(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            try
            {
                checked
                {
                    self.doubleValue += v.doubleValue;
                }
            }
            catch (OverflowException ex)
            {
                // TODO: return raised value
            }
            return self;
        }

        private static LpObject minus(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            try
            {
                checked
                {
                    self.doubleValue -= v.doubleValue;
                }
            }
            catch (OverflowException ex) {
                // TODO: return raised value
            }
            return self;
        }

        private static LpObject mul(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            self.doubleValue *= v.doubleValue;
            return self;
        }

        private static LpObject div(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            self.doubleValue /= v.doubleValue;
            return self;
        }

        private static LpObject mod(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            self.doubleValue %= v.doubleValue;
            return self;
        }

        private static LpObject pow(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            self.doubleValue = Math.Pow( (double)self.doubleValue, (double)v.doubleValue );
            return self;
        }

        private static LpObject compareToGreater(LpObject self, LpObject args) {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.doubleValue > o.doubleValue);
        }

        private static LpObject compareToGreaterEqual(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.doubleValue >= o.doubleValue);
        }

        private static LpObject compareToLower(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.doubleValue < o.doubleValue);
        }

        private static LpObject compareToLowerEqual(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.doubleValue <= o.doubleValue);
        }

        private static LpObject equal(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.doubleValue == o.doubleValue);
        }

        private static LpObject eq(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.doubleValue == o.doubleValue);
        }

        private static LpObject between(LpObject self, LpObject args)
        {
            var v1  = args.arrayValues.ElementAt(0).doubleValue;
            var v2 = args.arrayValues.ElementAt(1).doubleValue;
            var v = self.doubleValue;
            var max = (v1 < v2) ? v2 : v1;
            var min = (v1 < v2) ? v1 : v2;
            return LpBool.initialize( min <= v && v<= max );
        }
    }
}
