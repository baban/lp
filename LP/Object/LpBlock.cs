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
            LpObject iobj = new LpObject();
            iobj.superclass = new LpObject();
            iobj.methods["to_s"] = new BinMethod(to_s);
            iobj.methods["display"] = new BinMethod(display);
            iobj.methods["execute"] = new BinMethod(execute);
            iobj.statements = new List<string>();

            return iobj;
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
