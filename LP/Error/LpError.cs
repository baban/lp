using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Error
{
    class LpError : Exception
    {
        static string className = "Error";

        private Stack<string> stackTrace = new Stack<string>();

        public Stack<string> BackTrace
        {
            get
            {
                return stackTrace;
            }
            set
            {
                stackTrace = value;
            }
        }
    }
}
