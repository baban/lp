using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpMethod : LpBase
    {
        public LpMethod(BinMethod m) {
            method = m;
        }

        public static LpObject initialize(BinMethod method)
        {
            var obj = LpObject.initialize();
            obj.class_name = "Method";
            obj.superclass = LpObject.initialize();
            obj.method = method;
            // TODO: display
            // TODO: inspect
            // TODO: to_s
            return obj;
        }

        public LpObject funcall( LpObject self, LpObject args ) {
            return method(self,args);
        }
        
    }
}
