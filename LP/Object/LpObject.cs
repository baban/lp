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
        static string className = "Object";

        public LpObject() {
            class_name = className;
        }

        public static LpObject initialize() {
            return createClassTemplate();
        }

        private static LpObject createClassTemplate()
        {
            if (classes.ContainsKey(className))
            {
                return classes[className].Clone();
            }
            else
            {
                LpObject obj = new LpObject();
                setMethods(obj);
                obj.superclass = null;
                obj.class_name = className;
                classes[className] = obj;
                return obj.Clone();
            }
        }

        static private LpObject setMethods( LpObject obj )
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
            obj.methods["inspect"] = new BinMethod(inspect);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["class"] = new BinMethod(_class);
            obj.methods["hash"] = new BinMethod(hash);
            return obj;
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

        public LpObject funcall(string name, LpObject args, LpObject block = null )
        {
            return funcall(name, this, args, null);
        }

        public LpObject funcall(string name, LpObject self, LpObject args, LpObject block )
        {

            name = trueFname(name);

            LpMethod m = null;
            if (methods[name] is BinMethod)
            {
                m = new LpMethod((BinMethod)methods[name]);
                return m.funcall(self, args);
            }
            else if (methods[name] is LpMethod)
            {
                m = (LpMethod)methods[name];
                return m.funcall(self, args);
            }

            if (null != superclass)
            {
                return superclass.funcall(name, self, args, block);
            }

            throw new Error.LpNoMethodError();
        }

        private string trueFname(string name) {
            if (name[0] == '(') name = name.Replace("(", "").Replace(")", "");

            return name;
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

        public LpObject Clone()
        {
            return (LpObject)this.MemberwiseClone();
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

    }
}
