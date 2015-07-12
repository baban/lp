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

        public LpArguments( string[] args, bool loose = false )
        {
            arguments = args.TakeWhile((s) => s[0] != '*' && s[0] != '&').ToArray();
            arrayArg = args.Where((s) => s[0] == '*').Select((s)=>s.TrimStart('*')).FirstOrDefault();
            blockArg = args.Where((s) => s[0] == '&').Select((s) => s.TrimStart('&')).FirstOrDefault();
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
            if (arity() < 0)
            {
                Array.Copy(args, dstArgs, argsSize-1);
                dstArgs[argsSize - 1] = Object.LpArray.initialize(args.Skip(argsSize-1).Take(args.Count() - (argsSize-1)).ToArray());
            }
            else
            {
                Array.Copy(args, dstArgs, argsSize);
            }

            return dstArgs;
        }

        public Object.LpObject setVariables(Object.LpObject self, Object.LpObject[] args, Object.LpObject block)
        {
            if (args.Count() < arguments.Count())
                throw new Error.LpArgumentError();

            for (int i = 0; i < arguments.Count(); i++)
            {
                self.variables[arguments[i]] = args[i];
            }

            if (arrayArg != null) {
                self.variables[arrayArg] = Object.LpArray.initialize(args.Skip(arguments.Count()).ToArray());
            }
            if (blockArg != null)
            {
                self.variables[blockArg] = block;
            }
            return self;
        }

    }
}
