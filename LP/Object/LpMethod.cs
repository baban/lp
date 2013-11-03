using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpMethod : LpObject
    {

        public static LpObject initialize(BinMethod method)
        {
            var obj = LpObject.initialize();
            obj.superclass = LpObject.initialize();
            return obj;
        }

        public LpObject funcall( string name, LpObject args ) {
            return null;
        }
    }
}
