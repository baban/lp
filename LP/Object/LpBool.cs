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

        public static LpObject initialize()
        {
            return createClassTemplate();
        }

        public static LpObject initialize(bool b)
        {
            return init(b);
        }

        public static LpObject initialize( string b )
        {
            return init( bool.Parse(b) );
        }

        private static LpObject init( bool b )
        {
            var obj = createClassTemplate();
            obj.boolValue = b;
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
            // TODO: &
            // TODO: |
            obj.methods["||"] = new LpMethod( new BinMethod(andOp), 1 );
            obj.methods["&&"] = new LpMethod( new BinMethod(orOp), 1 );

            obj.methods["=="] = new LpMethod( new BinMethod(equal), 1 );
            obj.methods["==="] = new LpMethod( new BinMethod(eq), 1 );

            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s), 0 );
            obj.methods["display"] = new LpMethod( new BinMethod(display), 0 );
            obj.methods["inspect"] = new LpMethod( new BinMethod(inspect), 0 );
        }

        private static LpObject andOp(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize((bool)self.boolValue && (bool)o.boolValue);
        }

        private static LpObject orOp(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize((bool)self.boolValue || (bool)o.boolValue);
        }

        private static LpObject equal(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize(self.boolValue == o.boolValue);
        }

        private static LpObject eq(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize(self.boolValue == o.boolValue);
        }

        protected static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize( self.boolValue==true ? "true" : "false" );
        }

        protected static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return to_s(self, args, null);
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = to_s(self, args);
            Console.WriteLine(v.stringValue);
            return LpNl.initialize();
        }
    }
}
