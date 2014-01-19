using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpObject : LpBase
    {


        public LpObject() {
            class_name = "Object";
            setMethods();
        }

        private void setMethods()
        {
            // TODO: send, __send__
            // TODO: copy
            // TODO: is_a?
            // TODO: tap
            // TODO: methods
            // TODO: hash
            // TODO: ==
            // TODO: ===
            // TODO: blank?
            // TODO: copy
            // TODO: eq?
            // TODO: equal?
            // TODO: instance_eval, do
            // TODO: is_a?, kind_of?
            // TODO: nil?
            // TODO: method_missing
            // TODO: extend
            // TODO: desine_method
            // TODO: define_operand
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
            Console.WriteLine("name:");
            Console.WriteLine(name);
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

        public LpObject Clone() {
            return (LpObject)this.MemberwiseClone();
        }
    }
}
