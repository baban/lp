using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpArguments : LpBase
    {
        public static LpObject initialize()
        {
            return init();
        }

        public static LpObject initialize(string[] args)
        {
            return null;
            /*
            return args.Aggregate(
                init(),
                (o, stmt) =>
                {
                    var v = LpParser.STMT.Parse(stmt);
                    o.funcall("push", v);
                    return o;
                });*/
        }

        public static LpObject init()
        {
            LpObject obj = new LpObject();
            obj.class_name = "Arguments";
            obj.superclass = LpObject.initialize();
            obj.arrayValues = new List<LpObject>();
            setMethods(obj);
            return obj;
        }

        static void setMethods(LpObject obj)
        {
            //obj.methods["push"] = new BinMethod(push);
            //obj.methods["first"] = new BinMethod(first);
            // TODO: display
            // TODO: inspect
            // TODO: to_s
            // TODO: size
        }

        static LpObject first(LpObject self, LpObject args)
        {
            return self.arrayValues.First();
        }

        static LpObject push(LpObject self, LpObject args)
        {
            self.arrayValues.Add(args);
            return self;
        }
    }
}
