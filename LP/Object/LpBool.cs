using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpBool : LpBase
    {
        static string className = "Bool";

        public static LpObject initialize( bool b )
        {
            return init(b);
        }

        public static LpObject initialize( string b )
        {
            return init( bool.Parse(b) );
        }

        private static LpObject init( bool b )
        {
            var obj = createTemplate();
            obj.boolValue = b;
            return obj;
        }

        private static LpObject createTemplate() {
            if (classes.ContainsKey(className))
            {
                return classes[className].Clone();
            }
            else
            {
                LpObject obj = LpObject.initialize();
                setMethods(obj);
                obj.superclass = LpObject.initialize();
                obj.class_name = className;
                classes[className] = obj;
                return obj.Clone();
            }
        }


        private static void setMethods(LpObject obj)
        {
            // TODO: &
            // TODO: |
            obj.methods["||"] = new BinMethod(andOp);
            obj.methods["&&"] = new BinMethod(orOp);

            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);

            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
        }

        private static LpObject andOp(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize((bool)self.boolValue && (bool)o.boolValue);
        }

        private static LpObject orOp(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize((bool)self.boolValue || (bool)o.boolValue);
        }

        private static LpObject equal(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.boolValue == o.boolValue);
        }

        private static LpObject eq(LpObject self, LpObject args)
        {
            var o = args.arrayValues.ElementAt(0);
            return LpBool.initialize(self.boolValue == o.boolValue);
        }

        protected static LpObject to_s(LpObject self, LpObject args)
        {
            return LpString.initialize( self.boolValue==true ? "true" : "false" );
        }

        protected static LpObject inspect(LpObject self, LpObject args)
        {
            return to_s(self, args);
        }

        protected static LpObject display(LpObject self, LpObject args)
        {
            var v = to_s(self, args);
            Console.WriteLine(v.stringValue);
            return null;
        }
    }
}
