using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpModule : LpBase
    {
        static string className = "Module";

        public static LpObject initialize()
        {
            return init( className, new List<string>());
        }

        private static LpObject init(string className, List<string> stmts)
        {
            LpObject obj = createClassTemplate(className);
            obj.class_name = className;
            //obj.statements = stmts.ToList();

            classes[obj.class_name] = obj;
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["new"] = new LpMethod(new BinMethod(new_), 1);
            /*
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["len"] = new BinMethod(len);

            obj.methods["<<"] = new BinMethod(add);
            obj.methods["+"] = new BinMethod(plus);

            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);
             */
        }

        private static LpObject createClassTemplate(string className)
        {
            if (className != "Module")
            {
                return LpModule.initialize();
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
                //Util.LpIndexer.push(self);
                block.funcall("call", block, new LpObject[] { }, null);
                //Util.LpIndexer.pop();
            }

            klass.methods["new"] = new LpMethod(new BinMethod(initialize), -1);

            return klass;
        }

        public static LpObject initialize(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self;
        }
    
    }
}
