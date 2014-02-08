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
            //obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            //obj.methods["display"] = new LpMethod(new BinMethod(display));
            //obj.methods["execute"] = new LpMethod( new BinMethod(execute) );
            //obj.methods["call"] = new LpMethod(new BinMethod(execute));
        }

        static LpObject to_s(LpObject self, LpObject args)
        {
            self.stringValue = self.ToString();
            return self;
        }

        static LpObject display(LpObject self, LpObject args)
        {
            var so = to_s(self, args);
            Console.WriteLine(so.stringValue);
            return null;
        }

        static LpObject execute(LpObject self, LpObject args)
        {
            LpObject ret = null;
            self.statements.ForEach(delegate(string stmt)
            {
                ret = LpParser.STMT.Parse(stmt);
            });
            return ret;
        }
    }
}
