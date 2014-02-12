using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpBlock : LpBase
    {
        static string className = "Block";

        public static LpObject initialize()
        {
            return init();
        }

        static LpObject init()
        {
            var obj = createClassTemplate();
            obj.statements = new List<string>();
            return obj;
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
            obj.arguments = new Util.LpArguments( (string[])args[0], (bool)args[1] );
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
            // TODO: inspect
            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            obj.methods["display"] = new LpMethod( new BinMethod(display) );
            // TODO: to_block
            // TODO: to_method
            // TODO: to_lambda
            obj.methods["call"] = new LpMethod(new BinMethod(call));
        }

        static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            self.stringValue = self.ToString();
            return self;
        }

        static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = to_s(self, args, null);
            Console.WriteLine(o.stringValue);
            return null;
        }

        static LpObject call(LpObject self, LpObject[] args, LpObject block = null)
        {
            LpObject ret = LpNl.initialize();
            self.statements.ForEach(delegate(string stmt)
            {
                ret = LpParser.STMT.Parse(stmt);
            });
            return ret;
        }
    }
}
