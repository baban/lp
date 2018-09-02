using System;
using Irony.Interpreter.Ast;

namespace LP.Object
{
    class LpQuote : LpBase
    {
        static string className = "Quote";

        public static LpObject initialize()
        {
            return init(null);
        }

        public static LpObject initialize(AstNode node)
        {
            return init(node);
        }

        private static LpObject init(AstNode node)
        {
            LpObject obj = createClassTemplate();
            obj.statements = node;
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
                obj.is_macro = true;
                obj.stringValue = "";
                classes[className] = obj;
                return obj.Clone();
            }
        }

        
        private static void setMethods( LpObject obj )
        {
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect), 0);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
        }

        private static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            //return LpString.initialize( self.statements.First().toSource() );
            return null;
        }

        private static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            //return LpString.initialize(string.Format("'{0}", self.statements.First().toSource()));
            return null;
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine( to_s(self, args, block).stringValue );
            return LpNl.initialize();
        }
    }
}
