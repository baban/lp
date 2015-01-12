using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpMacro : LpBase
    {
        static string className = "Macro";

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
            obj.methods["new"] = new LpMethod(new BinMethod(new_), 0);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect), 0);
            obj.methods["call"] = new LpMethod(new BinMethod(call), -1);
            obj.methods["bind"] = new LpMethod(new BinMethod(bind), 1);
            /*
            obj.methods["=="] = new LpMethod(new BinMethod(equal), 1);
            obj.methods["==="] = new LpMethod(new BinMethod(eq), 1);
             */
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
                classes[className] = obj;
                return obj.Clone();
            }
        }

        private static LpObject new_(LpObject self, LpObject[] args, LpObject block = null)
        {
            if (block == null)
                throw new Error.LpArgumentError();

            var obj = self.Clone();
            obj.statements = block.statements;
            obj.arguments = block.arguments;
            return obj;
        }

        public static LpObject call(LpObject self, LpObject[] args, LpObject block = null)
        {
            var dstArgs = (null == args || args.Count() == 0) ? new LpObject[] { } : args.First().arrayValues.ToArray();
            self.arguments.setVariables(self, dstArgs, block);

            LpObject ret = Object.LpNl.initialize();
            foreach (Ast.LpAstNode stmt in self.statements)
            {
                ret = stmt.Evaluate(true);
                if (ret.class_name == "Quote" || ret.class_name == "QuasiQuote")
                    ret = LpParser.execute(ret.stringValue);
            }
            return ret;
        }

        static Object.LpObject bind(LpObject self, LpObject[] args, LpObject block = null)
        {
            var name = args[0].stringValue;
            var ctx = Util.LpIndexer.getLatestClass();
            ctx.methods[name] = self;
            return self;
        }

        private static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            var stmt = self.statements.First();
            var str = self.statements.First().toSource();
            return LpString.initialize(str);
        }
         
        private static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(string.Format("Macro.new({0})", self.stringValue));
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine(self.stringValue);
            return LpNl.initialize();
        }
    }
}
