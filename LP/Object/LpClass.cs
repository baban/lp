using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace LP.Object
{
    class LpClass : LpBase
    {
        static string className = "Class";

        public static LpObject initialize()
        {
            return init( className, new List<Ast.LpAstNode>() );
        }

        public static LpObject initialize( List<Ast.LpAstNode> stmts )
        {
            return init( className, stmts );
        }

        public static LpObject initialize(string className, List<Ast.LpAstNode> stmts)
        {
            return init(className, stmts);
        }

        private static LpObject init(string className, List<Ast.LpAstNode> stmts)
        {
            LpObject obj = createClassTemplate( className );
            obj.class_name = className;
            obj.statements = null; // stmts.ToList();

            classes[obj.class_name] = obj;
            //Util.LpIndexer.push(obj);

            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["new"] = new LpMethod(new BinMethod(new_), 1);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect),0);
            /*
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
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
                return LpClass.initialize();
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
                Util.LpIndexer.push(klass);
                block.funcall("call", block, new LpObject[] { }, null);
                Util.LpIndexer.pop();
            }
            klass.methods["new"] = new LpMethod(new BinMethod(initialize), -1);

            return klass;
        }

        private static LpObject create_mold_class( string class_name ) {
            LpObject kls = LpClass.initialize().Clone();
            kls.class_name = class_name;
            kls.methods = (Hashtable)kls.methods.Clone();
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
    }
}
