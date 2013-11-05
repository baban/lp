using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpString : LpObject
    {
        public static LpObject initialize() {
            return init( "" );
        }
        
        public static LpObject initialize( string s )
        {
            return init(s);
        }
        
        private static LpObject init(string s)
        {
            LpObject obj = LpObject.initialize();
            //obj.superclass = LpObject.initialize();
            obj.stringValue = s;
            return obj;
        }

        protected static LpObject to_s(LpObject self, LpObject args)
        {
            return self;
        }


    }
}
