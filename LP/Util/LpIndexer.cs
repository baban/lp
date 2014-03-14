using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Util
{
    class LpIndexer
    {
        public static Stack<Object.LpObject> contextStack = new Stack<Object.LpObject>();
        public static Object.LpObject currentContext = null;

        public static Object.LpObject set(string name, Object.LpObject value)
        {
            return currentContext.setVariable(name, value);
        }

        public static Object.LpObject get(string name)
        {
            return currentContext.getVariable(name);
        }

        public static Object.LpObject push(Object.LpObject context)
        {
            contextStack.Push(context);
            return context;
        }

        public static Object.LpObject pop()
        {
            return contextStack.Pop();
        }

        public static Object.LpObject last()
        {
            if (contextStack.Count() == 0)
                return Object.LpKernel.initialize();

            return contextStack.Last();
        }
    }
}
