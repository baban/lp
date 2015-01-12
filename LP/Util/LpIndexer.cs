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

        public static void initialize()
        {
            contextStack = new Stack<Object.LpObject>();
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
            contextStack.Push(context);
            currentContext = context;
            return context;
        }

        public static Object.LpObject pop()
        {
            return contextStack.Pop();
        }

        public static Object.LpObject last()
        {
            if (contextStack.Count() == 0) {
                currentContext = Object.LpKernel.initialize();
                return currentContext;
            } else {
                currentContext = contextStack.First();
                return currentContext;
            }
        }

        public static Object.LpObject getLatestClass()
        {
            foreach (var o in contextStack)
                if (o.class_name == "Kernel" || o.class_name == "Class")
                    return o;

            return null;
        }

        public static Object.LpObject loadfunc(string name) {
            foreach (var ctx in contextStack)
            {
                var ret = ctx.varcall(name);
                return ret;
            }
            return null;
        }

        public static Object.LpObject macrocall(string name) {
            foreach (var ctx in contextStack)
            {
                var ret = ctx.varcall(name);
                return ret;
            }
            return null;
        }

        public static Object.LpObject varcall( string name ){
            foreach (var ctx in contextStack)
            {
                var ret = ctx.varcall(name);
                return ret;
            }
            throw new Error.NameError();
        }

        public static int depth()
        {
            return contextStack.Count();
        }
    }
}
