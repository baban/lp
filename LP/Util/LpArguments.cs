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
        string arrayArg = null;
        string blockArg = null;
        int? arityNumber = null;

        public LpArguments() {
            arguments = new string[] { };
        }

        public LpArguments( int arity )
        {
            arityNumber = arity;
        }

        public LpArguments(string[] args, bool loose = false)
        {
            arrayArg = args.FirstOrDefault((s) => s[0] == '*');
            blockArg = args.FirstOrDefault((s) => s[0] == '&');
            arguments = args.TakeWhile((s) => s[0] != '*' && s[0] != '&' ).ToArray();
            this.loose = loose;
        }

        public int arity()
        {
            if (null == arityNumber) {
                int cnt = arguments.Count();
                if (arrayArg != null) cnt = -1 * (cnt + 1);
                arityNumber = cnt;
            }
            return (int)arityNumber;
        }

        public bool check( Object.LpObject[] args )
        {
            return (arity() < 0) ?
                args.Count() >= Math.Abs(arity()) :
                args.Count() == arity();
        }

        public Object.LpObject[] putVariables(Object.LpObject[] args, Object.LpObject block)
        {
            if (args == null) return new Object.LpObject[] { };

            if (args.Count() == arity()) return args;

            int argsSize = Math.Abs(arity());

            Object.LpObject[] dstArgs = new Object.LpObject[argsSize];
            Array.Copy(args, dstArgs, argsSize);

            if( arity() < 0 )
                dstArgs[dstArgs.Count()] = Object.LpArray.initialize(args.Skip(argsSize).Take(args.Count() - argsSize).ToArray());

            return dstArgs;
        }

        public Object.LpObject setVariables(Object.LpObject self, Object.LpObject[] args, Object.LpObject block)
        {
            for( int i = 0; i < arguments.Count(); i++ ){
                self.variables[arguments[i]] = args[i];
            }
            return self;
        }

    }
}
