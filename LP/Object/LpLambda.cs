using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpLambda : LpBase
    {
        static string className = "Lambda";

        public static LpObject initialize()
        {
            return init();
        }

        public static LpObject initialize(string s)
        {
            LpObject obj = init();
            obj.statements.Add(s);
            return obj;
        }

        public static LpObject initialize(string[] stmts)
        {
            LpObject obj = init();
            obj.statements = stmts.ToList();
            return obj;
        }

        public static LpObject initialize(string[] stmts, object[] args)
        {
            LpObject obj = init();
            obj.statements = stmts.ToList();
            obj.arguments = new Util.LpArguments((string[])args[0], (bool)args[1]);
            return obj;
        }

        static LpObject init()
        {
            LpObject obj = createClassTemplate();
            obj.statements = new List<string>();
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
            // TODO: display
            // TODO: inspect
            // TODO: to_s
            // TODO: call
            // TODO: to_block
            // TODO: to_method
            // TODO: to_lambda
            //obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            //obj.methods["display"] = new LpMethod(new BinMethod(display));
            obj.methods["call"] = new LpMethod(new BinMethod(call));
        }

        static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            self.stringValue = self.ToString();
            return self;
        }

        static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var so = to_s(self, args);
            Console.WriteLine(so.stringValue);
            return null;
        }

        static LpObject call(LpObject self, LpObject[] args, LpObject block = null)
        {
            LpObject ret = null;
            foreach (string stmt in self.statements) {
                ret = LpParser.STMT.Parse(stmt);
                if (control_status == (int)LpBase.CONTROL_CODE.RETURN)
                {
                    control_status = (int)LpBase.CONTROL_CODE.NONE;
                    return ret;
                }
            }
            return ret;
        }
    }
}
