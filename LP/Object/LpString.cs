using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpString : LpObject
    {
        public static LpObject initialize() {
            return init( "" );
        }
        
        public static LpObject initialize( string s )
        {
            return init(s);
        }
        
        private static LpObject init(string s)
        {
            LpObject obj = LpObject.initialize();
            setMethods(obj);
            obj.superclass = LpObject.initialize();
            obj.stringValue = s;
            obj.class_name = "string";
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["to_s"]    = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["size"] = new BinMethod(size);

            obj.methods["<<"] = new BinMethod(add);
            obj.methods["+"]  = new BinMethod(plus);

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

        protected static LpObject size(LpObject self, LpObject args)
        {
            return LpNumeric.initialize( self.stringValue.Length );
        }

        protected static LpObject add(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            self.stringValue += v.stringValue;
            return self;
        }

        protected static LpObject plus(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            return init(self.stringValue + v.stringValue);
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
            return LpBool.initialize(self.stringValue == o.stringValue );
        }
    }
}
