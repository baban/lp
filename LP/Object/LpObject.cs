﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LP.Object
{
    public class LpObject : LpBase
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
                obj.superclass = LpKernel.initialize();
                obj.class_name = className;
                classes[className] = obj;
                return obj.Clone();
            }
        }

        static private void setMethods( LpObject obj )
        {
            // Lv0
            // TODO: method_missing
            // TODO: define_operand
            // TODO: super_method
            // TODO: set variable
            // TODO: get variable
            // TODO: set const variable
            // TODO: get const variable
            obj.methods["__send__"] = new LpMethod(new BinMethod(send), -1);
            obj.methods["class"] = new LpMethod(new BinMethod(class_), 0);
            obj.methods["eq?"] = new LpMethod(new BinMethod(eq), 1);
            obj.methods["equal?"] = new LpMethod(new BinMethod(equal), 1);
            obj.methods["dup"] = new LpMethod(new BinMethod(copy), 0);
            obj.methods["define_method"] = new LpMethod(new BinMethod(define_method), 1);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
            obj.methods["extend"] = new LpMethod(new BinMethod(extend), 1);
            obj.methods["hash"] = new LpMethod(new BinMethod(hash), 0);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect), 0);
            obj.methods["instance_eval"] = new LpMethod(new BinMethod(instance_eval), 1);
            obj.methods["nil?"] = new LpMethod(new BinMethod(is_nil), 0 );
            obj.methods["to_bool"] = new LpMethod(new BinMethod(to_bool), 0);
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["methods"] = new LpMethod(new BinMethod(methods_), 0);

            // Lv1
            // TODO: blank?
            // TODO: tap
            obj.methods["alias"] = new LpMethod(new BinMethod(alias), 2);
            obj.methods["do"] = new LpMethod(new BinMethod(instance_eval), 0);
            obj.methods["=="] = new LpMethod(new BinMethod(eq), 1);
            obj.methods["==="] = new LpMethod(new BinMethod(equal), 1);
            obj.methods["is_a?"] = new LpMethod(new BinMethod(is_a), 1);
            obj.methods["kind_of?"] = new LpMethod(new BinMethod(is_a), 1 );
            obj.methods["send"] = new LpMethod(new BinMethod(send), -1);
        }

        public override string ToString()
        {
            return string.Format("#<{0}:{1:x8}>", "Object", GetHashCode());
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj == this;
        }

        public LpObject funcall(string name, LpObject[] args, LpObject block = null)
        {
            return funcall(name, this, args, block);
        }

        public LpObject funcall(string name, LpObject self, LpObject[] args, LpObject block )
        {
            LpObject ret = execMethod(name, self, args, block);

            if (ret != null) return ret;

            LpMethod m = null;
            // method_missing
            m = methods["method_missing"] as LpMethod;
            if ( null != m )
                return m.funcall(self, args, block);
                
            // superclass
            if (null != superclass)
                return superclass.funcall(name, self, args, block);

            throw new Error.LpNoMethodError();
        }

        public LpMethod searchMethod(string name, LpObject self) {

            LpMethod m = (LpMethod)self.methods[name];

            if (null != m)
                return m;

            // method_missing
            m = methods["method_missing"] as LpMethod;
            if (null != m)
                return m;

            // superclass
            if (null != superclass)
                return superclass.searchMethod(name, self);

            return null;
        }

        public bool isMethodExist(string name, LpObject self, LpObject[] args, LpObject block) {
            if(methods[name] != null) return true;
            if(methods["method_missing"] != null) return false;
            return false;
        }

        /// <summary>
        /// 現在の文脈中にあるメソッドを実行する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="self"></param>
        /// <param name="args"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public LpObject execMethod(string name, LpObject self, LpObject[] args, LpObject block)
        {
            if (self.isBinaryClass && self.isMethodCached == false)
            {
                cacheReflectMethods(self);
            }

            if (null == methods[name]) return null;

            return doMethod(methods[name], self, args, block);
        }

        void cacheReflectMethods(LpObject self)
        {
            var className = "System.Console";
            Type type = Type.GetType(className);
            var ms = type.GetMethods();

            Dictionary<string, List<System.Reflection.MethodInfo>> infos = new Dictionary<string, List<System.Reflection.MethodInfo>>();
            ms.ToList().ForEach((a) =>
            {
                if (!infos.ContainsKey(a.Name))
                    infos[a.Name] = new List<System.Reflection.MethodInfo>();
                infos[a.Name].Add(a);
            });

            infos.ToList().ForEach((info) => methods[info.Key] = new LpMethod(info.Value) );
            self.isMethodCached = true;
        }

        public LpObject doMethod(System.Object method, LpObject self, LpObject[] args, LpObject block)
        {
            LpMethod m = null;

            if (null != (m = method as LpMethod))
            {
                return m.funcall(self, args, block);
            }
            else
            {
                var klass = (method as LpObject);
                switch (klass.class_name)
                {
                    case "Macro":
                        return LpMacro.call((LpObject)method, args, block);
                    case "Lambda":
                    case "Block":
                        return LpLambda.call((LpObject)method, args, block);
                    default:
                        return null;
                }
            }
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

        protected static LpObject alias(LpObject self, LpObject[] args, LpObject block = null)
        {
            string src = args[0].stringValue;
            string dst = args[1].stringValue;
            self.methods[dst] = (LpMethod)self.methods[src];
            return LpSymbol.initialize(dst);
        }

        protected static LpObject class_(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(self.class_name);
        }

        private static LpObject copy(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self.Clone();
        }

        private static LpObject extend(LpObject self, LpObject[] args, LpObject block = null)
        {
            var tmp = self.superclass;

            var src = args[0];
            src.superclass = tmp;

            self.superclass = src;
            return self;
        }

        private static LpObject equal(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize(o.GetHashCode() == o.GetHashCode());
        }

        private static LpObject eq(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return LpBool.initialize(o.GetHashCode() == o.GetHashCode());
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var so = to_s(self, args);
            Console.WriteLine(so.stringValue);
            return null;
        }

        protected static LpObject define_method(LpObject self, LpObject[] args, LpObject block = null)
        {
            var name = args[0].stringValue;
            //self.methods[name] = LpMethod.initialize( block.arguments, block.statements.ToList() );
            return LpSymbol.initialize( name );
        }


        protected static LpObject hash(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNumeric.initialize(self.GetHashCode());
        }

        protected static LpObject is_a(LpObject self, LpObject[] args, LpObject block = null)
        {
            if (self.class_name == args[0].class_name)
                return LpBool.initialize(true);

            if (null != self.superclass)
                return self.superclass.funcall("is_a?", args, block);

            return LpBool.initialize(false);
        }

        protected static LpObject is_nil(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpBool.initialize(false);
        }

        protected static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return to_s(self, args, block);
        }

        protected static LpObject instance_eval(LpObject self, LpObject[] args, LpObject block = null)
        {
            return block.funcall("call",null,null);
        }

        protected static LpObject methods_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var keys = new List<string>();
            foreach (string k in self.methods.Keys) {
                keys.Add(k);
            }
            return LpArray.initialize( keys.ToArray().Select((s) => LpString.initialize(s)).ToArray()  );
        }

        protected static LpObject send(LpObject self, LpObject[] args, LpObject block = null)
        {
            var methodName = args[0].stringValue;
            var sendedArgs = args.Skip(1).Take(args.Length - 1).ToArray();
            return self.funcall(methodName, sendedArgs, block);
        }

        protected static LpObject to_s(LpObject self, LpObject[] args = null, LpObject block = null)
        {
            var hashCode = self.GetHashCode();
            var str = string.Format("<obj {0}>", hashCode.ToString("x4"));
            return LpString.initialize(str);
        }

        protected static LpObject to_bool(LpObject self, LpObject[] args, LpObject block = null)
        {
            if(self.class_name=="Nl")
                return LpBool.initialize(false);
            if (self.class_name == "Bool" )
                return LpBool.initialize((bool)self.boolValue);

            return LpBool.initialize(true);
        }

        internal LpObject macrocall(string p, string[] args, LpObject lpObject)
        {
            throw new NotImplementedException();
        }
    }
}
