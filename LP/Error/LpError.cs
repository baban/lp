using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Error
{
    class LpError : Exception
    {
        private Stack<string> stackTrace = new Stack<string>();

        public LpError() { }

        public LpError( string message, Exception e )
        {
        }

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
