using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Util
{
    class LpArguments
    {
        string[] arguments = null;
        bool loose = false;

        public LpArguments() {
            arguments = new string[] { };
        }

        public LpArguments( string[] args, bool loose = false )
        {
            arguments = args;
            this.loose = loose;
        }

        public int arity() {
            return arguments.Count();
        }

    }
}
