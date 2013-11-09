using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpMethod : LpObject
    {
        //BinMethod method = null;
        public static LpObject initialize(BinMethod method)
        {
            //var obj = LpObject.initialize();
            //obj.superclass = LpObject.initialize();
            //obj.method = method;
            return null;
        }

        public LpObject funcall( LpObject self, LpObject args ) {
            Console.WriteLine("funcall2");
            return null;
        }
        
    }
}
