﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LP.Object
{
    class LpNumeric : LpBase
    {
        static string className = "Numeric";

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
            var obj = createClassTemplate();
            obj.doubleValue = i;
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

        private static void setMethods( LpObject obj )
        {
            obj.methods["+"]  = new LpMethod( new BinMethod(plus) );
            obj.methods["-"]  = new LpMethod( new BinMethod(minus) );
            obj.methods["*"]  = new LpMethod( new BinMethod(mul) );
            obj.methods["/"]  = new LpMethod( new BinMethod(div) );
            obj.methods["%"]  = new LpMethod( new BinMethod(mod) );
            obj.methods["**"] = new LpMethod( new BinMethod(pow) );

            obj.methods[">"]        = new LpMethod( new BinMethod(compareToGreater) );
            obj.methods[">="]       = new LpMethod( new BinMethod(compareToGreaterEqual) );
            obj.methods["<"]        = new LpMethod( new BinMethod(compareToLower) );
            obj.methods["<="]       = new LpMethod( new BinMethod(compareToLowerEqual) );
            obj.methods["=="]       = new LpMethod( new BinMethod(equal) );
            obj.methods["==="]      = new LpMethod( new BinMethod(eq) );
            obj.methods["between?"] = new LpMethod( new BinMethod(between) );

            obj.methods["to_i"] = new LpMethod( new BinMethod(to_i) );
            obj.methods["to_f"] = new LpMethod( new BinMethod(to_f) );
            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            obj.methods["display"] = new LpMethod( new BinMethod(display) );
            obj.methods["inspect"] = new LpMethod( new BinMethod(inspect) );
        }

        protected static LpObject display(LpObject self, LpObject[] args)
        {
            Console.WriteLine(self.doubleValue.ToString());
            return null;
        }

        protected static LpObject to_s(LpObject self, LpObject[] args)
        {
            return LpString.initialize(self.doubleValue.ToString());
        }

        protected static LpObject inspect(LpObject self, LpObject[] args)
        {
            Debug.WriteLine(self.doubleValue.ToString());
            return LpString.initialize(self.doubleValue.ToString());
        }

        protected static LpObject to_f(LpObject self, LpObject[] args)
        {
            self.doubleValue = self.doubleValue;
            return self;
        }

        protected static LpObject to_i(LpObject self, LpObject[] args)
        {
            self.doubleValue = (int)self.doubleValue;
            return self;
        }

        private static LpObject plus(LpObject self, LpObject[] args)
        {
            var v = args[0];
            self.doubleValue += v.doubleValue;
            return self;
        }

        private static LpObject minus(LpObject self, LpObject[] args)
        {
            var v = args[0];
            self.doubleValue -= v.doubleValue;
            return self;
        }

        private static LpObject mul(LpObject self, LpObject[] args)
        {
            var v = args[0];
            self.doubleValue *= v.doubleValue;
            return self;
        }

        private static LpObject div(LpObject self, LpObject[] args)
        {
            var v = args[0];
            self.doubleValue /= v.doubleValue;
            return self;
        }

        private static LpObject mod(LpObject self, LpObject[] args)
        {
            var v = args[0];
            self.doubleValue %= v.doubleValue;
            return self;
        }

        private static LpObject pow(LpObject self, LpObject[] args)
        {
            var v = args[0];
            self.doubleValue = Math.Pow( (double)self.doubleValue, (double)v.doubleValue );
            return self;
        }

        private static LpObject compareToGreater(LpObject self, LpObject[] args) {
            var o = args[0];
            return LpBool.initialize(self.doubleValue > o.doubleValue);
        }

        private static LpObject compareToGreaterEqual(LpObject self, LpObject[] args)
        {
            var o = args[0];
            return LpBool.initialize(self.doubleValue >= o.doubleValue);
        }

        private static LpObject compareToLower(LpObject self, LpObject[] args)
        {
            var o = args[0];
            return LpBool.initialize(self.doubleValue < o.doubleValue);
        }

        private static LpObject compareToLowerEqual(LpObject self, LpObject[] args)
        {
            var o = args[0];
            return LpBool.initialize(self.doubleValue <= o.doubleValue);
        }

        private static LpObject equal(LpObject self, LpObject[] args)
        {
            var o = args[0];
            return LpBool.initialize(self.doubleValue == o.doubleValue);
        }

        private static LpObject eq(LpObject self, LpObject[] args)
        {
            var o = args[0];
            return LpBool.initialize(self.doubleValue == o.doubleValue);
        }

        private static LpObject between(LpObject self, LpObject[] args)
        {
            var v1 = args[0].doubleValue;
            var v2 = args[1].doubleValue;
            var v = self.doubleValue;
            var max = (v1 < v2) ? v2 : v1;
            var min = (v1 < v2) ? v1 : v2;
            return LpBool.initialize( min <= v && v<= max );
        }
    }
}
