using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpFile : LpBase
    {
        public static LpObject initialize()
        {
            return init();
        }

        private static LpObject init()
        {
            LpObject obj = LpObject.initialize();
            setMethods(obj);
            obj.superclass = LpObject.initialize();
            obj.class_name = "File";
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            // TODO: new, open
            // TODO: read
            // TODO: <<
            // TODO: write
        }
    }
}
