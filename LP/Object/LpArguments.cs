using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpArguments : LpBase
    {
        public static LpObject initialize()
        {
            LpObject obj = new LpObject();
            LpObject iobj = new LpObject();
            iobj.class_name = "Arguments";
            iobj.superclass = obj;
            iobj.arrayValues = new List<LpObject>();
            setMethods(iobj);
            return iobj;
        }

        static void setMethods(LpObject obj)
        {
            obj.methods["push"] = new BinMethod(push);
            obj.methods["first"] = new BinMethod(first);
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
