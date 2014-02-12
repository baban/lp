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

        static private void setMethods( LpObject obj )
        {
            // Lv0
            // TODO: copy
            // TODO: ==
            // TODO: ===
            // TODO: eq?
            // TODO: equal?
            // TODO: instance_eval, do
            // TODO: method_missing
            // TODO: method_matching
            // TODO: extend
            // TODO: define_operand
            // TODO: super
            obj.methods["copy"] = new LpMethod(new BinMethod(copy));
            obj.methods["extend"] = new LpMethod(new BinMethod(extend));
            obj.methods["instance_eval"] = new LpMethod(new BinMethod(instance_eval));
            obj.methods["__send__"] = new LpMethod(new BinMethod(send));
            obj.methods["methods"] = new LpMethod(new BinMethod(_methods));
            obj.methods["define_method"] = new LpMethod(new BinMethod(define_method));
            obj.methods["nil?"] = new LpMethod(new BinMethod(is_nil));
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect));
            obj.methods["display"] = new LpMethod( new BinMethod(display) );
            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            obj.methods["class"] = new LpMethod( new BinMethod(_class) );
            obj.methods["hash"] = new LpMethod( new BinMethod(hash) );

            // Lv1
            // TODO: blank?
            // TODO: tap
            obj.methods["is_a?"] = new LpMethod(new BinMethod(is_a));
            obj.methods["send"] = new LpMethod(new BinMethod(send));
            obj.methods["kind_of?"] = new LpMethod(new BinMethod(is_a));
            obj.methods["do"] = new LpMethod(new BinMethod(instance_eval));
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

        public LpObject funcall(string name, LpObject[] args,  LpObject block=null )
        {
            return funcall(name, this, args, block);
        }

        public LpObject funcall(string name, LpObject self, LpObject[] args, LpObject block )
        {
            name = trueFname(name);

            LpMethod m = null;
            // normal search
            m = methods[name] as LpMethod;
            if ( null!= m )
                return m.funcall(self, args, block);

            // method_missing
            m = methods["method_missing"] as LpMethod;
            if ( null != m )
                return m.funcall(self, args, block);

            // superclass
            if (null != superclass)
                return superclass.funcall(name, self, args, block);

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
        
        protected static LpObject define_method(LpObject self, LpObject[] args, LpObject block = null)
        {
            var name = args[0].stringValue;
            self.methods[name] = LpMethod.initialize( block.arguments, block.statements.ToArray());
            return LpSymbol.initialize( name );
        }

        protected static LpObject send(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self.funcall(args[0].stringValue, args, block);
        }

        protected static LpObject alias(LpObject self, LpObject[] args, LpObject block = null)
        {
            string src = args[0].stringValue;
            string dst = args[1].stringValue;
            self.methods[dst] = (LpMethod)self.methods[src];
            return Object.LpSymbol.initialize(dst);
        }

        protected static LpObject _methods(LpObject self, LpObject[] args, LpObject block = null)
        {
            var keys = new string[] { };
            return LpArray.initialize( keys );
        }

        protected static LpObject is_nil(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpBool.initialize(false);
        }

        protected static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(self.ToString());
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var so = to_s( self, args );
            Console.WriteLine( so.stringValue );
            return null;
        }

        protected static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return to_s( self, args, block );
        }

        protected static LpObject _class(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(self.class_name);
        }

        protected static LpObject hash(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNumeric.initialize( self.GetHashCode() );
        }

        protected static LpObject instance_eval(LpObject self, LpObject[] args, LpObject block = null)
        {
            // TODO
            return null;
        }

        protected static LpObject is_a(LpObject self, LpObject[] args, LpObject block = null)
        {
            if (self.class_name == args[0].class_name)
                return LpBool.initialize(true);

            if (null != self.superclass)
                return self.superclass.funcall("is_a?", args, block);

            return LpBool.initialize(false);
        }

        // TODO: 全く未実装
        private static LpObject _super(LpObject self, LpObject[] args, LpObject block = null)
        {
            var superclass = self.superclass;
            if (superclass != null)
            {
                // 自分の文脈を取得＆同じメソッドを呼び出し
            }
            return null;
        }

        // TODO: 全く未実装
        private static LpObject copy(LpObject self, LpObject[] args, LpObject block = null)
        {
            return null;
        }

        // TODO: 全く未実装
        private static LpObject extend(LpObject self, LpObject[] args, LpObject block = null)
        {
            return null;
        }
    }
}
