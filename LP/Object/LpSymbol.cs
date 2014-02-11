using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpSymbol : LpBase
    {
        protected static Dictionary<string, LpObject> symbols = new Dictionary<string, LpObject>();
        static string className = "Symbol";

        public static LpObject initialize(string s)
        {
            return init(s);
        }

        private static LpObject init(string s)
        {
            if (symbols.ContainsKey(s)) return symbols[s];
            var obj = createClassTemplate();
            obj.stringValue = s;
            symbols[s] = obj;
            obj.doubleValue = symbols[s].GetHashCode();
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
            obj.methods["inspect"] = new LpMethod( new BinMethod(inspect) );
            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            obj.methods["display"] =new LpMethod(  new BinMethod(display) );

            obj.methods["="] = new LpMethod( new BinMethod(setOp) );
            obj.methods["=="] = new LpMethod( new BinMethod(equal) );
            obj.methods["==="] = new LpMethod( new BinMethod(eq) );
        }

        protected static LpObject to_s(LpObject self, LpObject[] args)
        {
            return self;
        }

        protected static LpObject inspect(LpObject self, LpObject[] args)
        {
            return self;
        }

        protected static LpObject display(LpObject self, LpObject[] args)
        {
            Console.WriteLine(self.stringValue);
            return null;
        }

        // TODO: =
        private static LpObject setOp(LpObject self, LpObject[] args)
        {
            var v = args[0];
            var env = LpIndexer.last();
            var o = env.setVariable(self.stringValue, v);
            return o;
        }

        private static LpObject equal(LpObject self, LpObject[] args)
        {
            return eq( self, args );
        }

        private static LpObject eq(LpObject self, LpObject[] args)
        {
            var o = args[0];
            var sym = symbols[o.stringValue];
            return LpBool.initialize( self == sym );
        }
    }
}
