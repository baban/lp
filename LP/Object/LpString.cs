﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpString : LpBase
    {
        static string className = "String";

        public static LpObject initialize()
        {
            return init( "" );
        }
        
        public static LpObject initialize( string s )
        {
            return init(s);
        }
        
        private static LpObject init(string s)
        {
            var obj = createClassTemplate();
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

        private static void setMethods(LpObject obj)
        {
            obj.methods["inspect"] = new LpMethod( new BinMethod(inspect) );
            obj.methods["to_s"]    = new LpMethod( new BinMethod(to_s) );
            obj.methods["display"] = new LpMethod( new BinMethod(display) );
            obj.methods["size"] = new LpMethod(new BinMethod(size));
            obj.methods["len"] = new LpMethod( new BinMethod(size) );

            obj.methods["<<"] = new LpMethod( new BinMethod(add) );
            obj.methods["+"]  = new LpMethod( new BinMethod(plus) );

            obj.methods["=="] = new LpMethod( new BinMethod(equal) );
            obj.methods["==="] = new LpMethod( new BinMethod(eq) );
        }

        protected static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self;
        }

        protected static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self;
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine(self.stringValue);
            return LpNl.initialize();
        }

        protected static LpObject size(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNumeric.initialize( self.stringValue.Length );
        }

        protected static LpObject add(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = args[0];
            self.stringValue += v.stringValue;
            return self;
        }

        protected static LpObject plus(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = args[0];
            return init(self.stringValue + v.stringValue);
        }

        private static LpObject equal(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize(self.stringValue == o.stringValue);
        }

        // TODO: This method must comare to memory address!!
        private static LpObject eq(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize(self.stringValue == o.stringValue );
        }
    }
}
