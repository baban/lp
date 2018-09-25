using System;
using Irony.Interpreter;
using Irony.Interpreter.Ast;

namespace LP.Object
{
    public class LpClass : LpBase
    {
        static string className = "Class";

        public static LpObject initialize()
        {
            return init( className, new AstNode() );
        }

        public static LpObject initialize(AstNode stmts)
        {
            return init( className, stmts );
        }

        public static LpObject initialize(string className, bool isBinary = false)
        {
            return init(className, new AstNode(), isBinary);
        }

        public static LpObject initialize(string className, AstNode stmts, bool isBinary = false, Scope scope = null)
        {
            return init(className, stmts);
        }

        private static LpObject init(string className, AstNode stmts, bool isBinary = false, Scope scope = null)
        {
            LpObject obj = createClassTemplate( className );
            obj.class_name = className;
            obj.statements = stmts;
            obj.isBinaryClass = isBinary;
            classes[obj.class_name] = obj;

            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["new"] = new LpMethod(new BinMethod(new_), 1);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect),0);
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
            /*
            obj.methods["size"] = new BinMethod(size);

            obj.methods["<<"] = new BinMethod(add);
            obj.methods["+"] = new BinMethod(plus);

            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);
             */
        }

        private static LpObject createClassTemplate(string className)
        {
            if (className != "Class") {
                return initialize();
            }

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

        public static LpObject new_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var class_name = args[0].stringValue;
            if (!classes.ContainsKey(class_name))
            {
                classes[class_name] = create_mold_class( class_name ) ;
            }

            LpObject klass = classes[class_name];

            if (null != block)
            {
                block.funcall("call", block, new LpObject[] { }, null);
            }
            klass.methods["new"] = new LpMethod(new BinMethod(initialize), -1);

            return klass;
        }

        private static LpObject create_mold_class( string class_name ) {
            LpObject kls = initialize().Clone();
            kls.class_name = class_name;
            //kls.methods = (Hashtable)kls.methods.Clone();
            return kls;
        }

        public static LpObject initialize(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self.Clone();
        }

        protected static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return to_s(self, args, block);
        }

        protected static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            var str = string.Format("#<{0}:{1:x8}>", className, self.GetHashCode());
            return LpString.initialize(str);
        }

        static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine(to_s(self, args, block).stringValue);
            return LpNl.initialize();
        }
    }
}
