using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpBlock : LpObject
    {
        public static LpObject initialize()
        {
            return init();
            // TODO: display
            // TODO: inspect
            // TODO: to_s
            // TODO: call
        }

        public static LpObject initialize(string s)
        {
            LpObject iobj = init();
            iobj.statements.Add(s);

            return iobj;
        }

        static LpObject init()
        {
            LpObject obj = new LpObject();
            obj.superclass = new LpObject();
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["execute"] = new BinMethod(execute);
            obj.statements = new List<string>();
            obj.class_name = "Block";

            return obj;
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
