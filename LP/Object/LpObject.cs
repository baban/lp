using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpObject
    {
        public LpObject superclass = null;
        public delegate LpObject BinMethod(LpObject self, LpObject args);
        protected Hashtable methods = new Hashtable();
        protected Hashtable variables = new Hashtable();

        public double? doubleValue = null;
        public string stringValue = null;
        public List<LpObject> arrayValues = new List<LpObject>();
        public List<string> statements = new List<string>();
        
        private void setMethods()
        {
            //methods["+"] = new MethodObj(plus);
            //methods["-"] = new MethodObj(minus);
            //methods["/"] = new MethodObj(times);
            //methods["*"] = new MethodObj(div);
            //methods["%"] = new MethodObj(mod);
            //methods[">"] = new MethodObj(compareTo);
            //methods["**"] = new MethodObj(pow);
            //methods["=="] = new MethodObj(equal);
            //methods["==="] = new MethodObj(eq);

            //methods["to_s"] = new BinMethod(to_s);
            //methods["display"] = new BinMethod(display);
            methods["to_s"] = LpMethod.initialize(to_s);
        }

        public override string ToString()
        {
            return string.Format("#<obj {0:x8}>", GetHashCode());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj == this;
        }

        public static LpObject initialize() {
            return new LpObject();
        }

        protected static LpObject to_s(LpObject self, LpObject args)
        {
            return LpString.initialize(self.ToString());
        }

        protected static LpObject display(LpObject self, LpObject args)
        {
            return null;
        }

        protected static LpObject inspect(LpObject self, LpObject args)
        {
            return null;
        }

        /*protected static LpObject to_s(LpObject self, LpObject args)
        {
            LpObject o = new LpString().initialize(self.ToString());
            return o;
        }*/

        /*
        public virtual LpObject display(LpObject self, LpObject args)
        {
            LpObject o = self.funcall("to_s", null);
            Console.WriteLine(o.stringValue);
            return o;
        }*/

        public LpObject funcall(String name, LpObject args)
        {
            if (null != methods[name])
            {
                return null;
            }
            return null;
        }
    }
}
