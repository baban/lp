using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpKernel
    {
        private static LpObject print(LpObject self, LpObject args)
        {
            var o = args.arrayValues.First();
            Console.WriteLine(o.funcall("to_s",null));
            return null;
        }

        // TODO: yield
        // TODO: return
        // TODO: super
        // TODO: while
        // TODO: until
        // TODO: if
        // TODO: until
        // TODO: alias
        // TODO: break
        // TODO: next
        // TODO: retry

        // 構文
        // TODO: self

        private static LpObject _if(LpObject self, LpObject args)
        {
            var o1 = args.arrayValues.ElementAt(0);
            if( o1!=null ){
                return args.arrayValues.ElementAt(1).funcall("call", null);
            } else {
                return args.arrayValues.ElementAt(2).funcall("call", null);
            }
        }
    }
}
