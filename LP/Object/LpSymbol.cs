﻿using System;
using System.Collections;
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
            if (symbols.ContainsKey(s)) return symbols[s];

            LpObject obj = LpObject.initialize();
            setMethods(obj);
            obj.superclass = LpObject.initialize();
            obj.stringValue = s;
            obj.class_name = "Symbol";
            symbols[s] = obj;
            obj.doubleValue = symbols[s].GetHashCode();
            return obj;
        }
        
        private static void setMethods(LpObject obj)
        {
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);

            obj.methods["="] = new BinMethod(setOp);
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

        // TODO: =
        private static LpObject setOp(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.stringValue == o.stringValue);
        }

        private static LpObject equal(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            var sym = symbols[o.stringValue];
            return LpBool.initialize(self.doubleValue != null && self.doubleValue == sym.doubleValue);
        }

        private static LpObject eq(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            var sym = symbols[o.stringValue];
            return LpBool.initialize( self.doubleValue!=null && self.doubleValue == sym.doubleValue );
        }
    }
}
