using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Util
{
    class LpIndexer
    {
        public static Queue<Object.LpObject> contextStack = new Queue<Object.LpObject>();
        public static Object.LpObject currentContext = null;

        public static void initialize()
        {
            Object.LpObject kernel = Object.LpObject.initialize();
            currentContext = kernel;
            contextStack.Enqueue(kernel);
        }

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
            contextStack.Enqueue(context);
            return context;
        }

        public static Object.LpObject pop(Object.LpObject context)
        {
            return contextStack.Dequeue();
        }

        public static Object.LpObject last()
        {
            return Object.LpKernel.initialize();
            //return contextStack.Last();
        }
    }
}
