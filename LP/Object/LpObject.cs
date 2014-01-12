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
        public BinMethod method = null;
        public delegate LpObject BinMethod(LpObject self, LpObject args);
        public Hashtable methods = new Hashtable();
        public Hashtable variables = new Hashtable();

        public bool? boolValue = null;
        public double? doubleValue = null;
        public string stringValue = null;
        public List<LpObject> arrayValues = null;
        public HashSet<LpObject> hashValues = null;
        public List<string> statements = null;

        public string class_name = null;

        public LpObject() {
            class_name = "Object";
            setMethods();
        }

        private void setMethods()
        {
            // TODO: send
            // TODO: copy
            // TODO: is_a?
            // TODO: tap
            // TODO: methods
            // TODO: hash
            methods["inspect"] = new BinMethod(inspect);
            methods["display"] = new BinMethod(display);
            methods["to_s"] = new BinMethod(to_s);
            methods["class"] = new BinMethod(_class);
            methods["hash"] = new BinMethod(hash);
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
            var so = to_s( self, args );
            Console.WriteLine( so.stringValue );
            return null;
        }

        protected static LpObject inspect(LpObject self, LpObject args)
        {
            return to_s( self, args );
        }

        protected static LpObject _class(LpObject self, LpObject args)
        {
            return LpString.initialize(self.class_name);
        }

        protected static LpObject hash(LpObject self, LpObject args)
        {
            return LpNumeric.initialize( self.GetHashCode() );
        }

        public LpObject funcall( string name, LpObject args )
        {
            if (null != methods[name])
            {
                var m = new LpMethod((BinMethod)methods[name]);
                return m.funcall(this,args);
            }
            return null;
        }

        public LpObject setVariable(String name, LpObject obj)
        {
            this.variables[name] = obj;
            return obj;
        }

        public LpObject getVariable(String name)
        {
            return (LpObject)this.variables[name];
        }
    }
}
