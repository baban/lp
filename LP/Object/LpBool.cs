using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpBool : LpObject
    {
        public static LpObject initialize( bool b )
        {
            return init(b);
        }

        public static LpObject initialize( string b )
        {
            return init( bool.Parse(b) );
        }

        private static LpObject init( bool b )
        {
            LpObject obj = LpObject.initialize();
            obj.superclass = LpObject.initialize();
            obj.class_name = "bool";
            obj.boolValue = b;
            return obj;
        }


    }
}
