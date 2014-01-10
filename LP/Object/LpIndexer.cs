using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpIndexer
    {
        public static Queue<LpObject> contextStack = new Queue<LpObject>();
        public static LpObject currentContext = null;

        public static void initialize()
        {
            LpObject kernel = LpObject.initialize();
            currentContext = kernel;
            contextStack.Enqueue(kernel);
        }

        public static LpObject set(string name, LpObject value)
        {
            return currentContext.setVariable(name, value);
        }

        public static LpObject get(string name)
        {
            return currentContext.getVariable(name);
        }

        public static LpObject push(LpObject context)
        {
            contextStack.Enqueue(context);
            return context;
        }

        public static LpObject pop(LpObject context)
        {
            return contextStack.Dequeue();
        }

        public static LpObject last()
        {
            return contextStack.Last();
        }
    }
}
