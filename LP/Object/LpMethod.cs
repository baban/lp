﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpMethod : LpObject
    {
        public LpMethod(BinMethod m) {
            method = m;
        }


        //BinMethod method = null;
        public static LpObject initialize(BinMethod method)
        {
            var obj = LpObject.initialize();
            obj.class_name = "method";
            obj.superclass = LpObject.initialize();
            obj.method = method;
            return obj;
        }

        public LpObject funcall( LpObject self, LpObject args ) {
            return method(self,args);
        }
        
    }
}
