using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Util
{
    class LpGlobalIndexer
    {
        public static Object.LpObject currentContext = new Object.LpObject();
        public static Object.LpObject set(string name, Object.LpObject value)
        {
            return currentContext.setVariable(name, value);
        }

        public static Object.LpObject get(string name)
        {
            return currentContext.getVariable(name);
        }
    }
}
