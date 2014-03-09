using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace LP.Object
{
    class LpQuote : LpBase
    {
        static string className = "Quote";

        public static LpObject initialize(string s)
        {
            return init(s);
        }

        private static LpObject init( string s )
        {
            LpObject obj = createClassTemplate();
            obj.stringValue = s;
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

        private static void setMethods( LpObject obj ) {
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
        }

        private static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize( self.stringValue );
        }
    }
}
