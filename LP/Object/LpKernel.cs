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

        private static LpObject _if(LpObject self, LpObject args)
        {
            var o1 = args.arrayValues.ElementAt(0);
            var o2 = args.arrayValues.ElementAt(1);
            var o3 = args.arrayValues.ElementAt(2);

            if( o1!=null ){
                return o2.funcall("call", null);
            } else {
                return o3.funcall("call", null);
            }
        }
    }
}
