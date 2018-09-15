using System;
using System.Linq;

namespace LP.Object
{
    public class LpNl : LpBase
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
            obj.methods["display"] = new LpMethod( new BinMethod(display), 0 );
            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s), 0 );
            obj.methods["inspect"] = new LpMethod( new BinMethod(inspect), 0 );
            obj.methods["to_i"] = new LpMethod( new BinMethod(to_i), 0 );
            obj.methods["=="] = new LpMethod( new BinMethod(equal), 1 );
            obj.methods["==="] = new LpMethod( new BinMethod(eq), 1 );
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

        protected static LpObject to_i(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNumeric.initialize(0);
        }

        protected static LpObject eq(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = args.First();
            return LpBool.initialize( v.class_name==self.class_name );
        }

        protected static LpObject equal(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = args.First();
            return LpBool.initialize(v.class_name == self.class_name);
        }

    }
}
