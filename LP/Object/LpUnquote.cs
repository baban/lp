using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LP.Object
{
    public class LpUnquote : LpBase
    {
        public LpObject initialize(string s)
        {
            LpObject o = new LpObject();
            //iobj.superclass = new LpString();
            //iobj.methods["to_s"] = new BinMethod(to_s);
            //iobj.methods["display"] = new BinMethod(display);

            o.stringValue = s;

            return o;
        }

        public static LpObject to_s(LpObject self, LpObject args)
        {
            self.stringValue = string.Format("{0}", self.doubleValue);
            return self;
        }

        public static LpObject display(LpObject self, LpObject args)
        {
            //return (LpObject)(new LpNl());
            return LpNl.initialize();
        }

        public static string expand(string name){
            return "10";
        }
    }
}
