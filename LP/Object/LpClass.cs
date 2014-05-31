using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            obj.statements = stmts.ToList();

            classes[obj.class_name] = obj;
            Util.LpIndexer.push(obj);

            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["new"] = new LpMethod(new BinMethod(new_), 1);
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
            var arg = args[0];
            if (!classes.ContainsKey(arg.stringValue))
            {
                LpObject kls = LpClass.initialize().Clone();
                kls.class_name = arg.stringValue;
                classes[arg.stringValue] = kls;
            }
            LpObject klass = classes[arg.stringValue];

            if (null != block)
            {
                block.funcall("call", block, new LpObject[]{}, null);
            }

            return klass;
        }

    }
}
