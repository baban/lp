using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpKernel : LpBase
    {
        static string className = "Kernel";

        public static LpObject initialize()
        {
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
                return obj;
            }
        }

        static private void setMethods(LpObject obj)
        {
            // TODO: rescue
            // TODO: redo
            // TODO: retry
            // TODO: raise

            // TODO: open

            // TODO: abort

            // 構文
            // TODO: self

            // TODO: block_given? // マクロで再現
            obj.methods["break"] = new LpMethod( new BinMethod(break_) );
            // TODO: caller
            // TODO: exec
            obj.methods["eval"] = new LpMethod(new BinMethod(eval));
            obj.methods["exit"] = new LpMethod(new BinMethod(exit));
            obj.methods["cond"] = new LpMethod(new BinMethod(cond), -1);
            obj.methods["loop"] = new LpMethod(new BinMethod(loop), 0);
            //obj.methods["next"] = new LpMethod( new BinMethod(next_) );
            //obj.methods["return"] = new LpMethod( new BinMethod(_return) );
            // TODO: require
            // TODO: yield // マクロで再現

            // Lv1
            obj.methods["print"] = new LpMethod(new BinMethod(print), 1);
            obj.methods["_if"] = new LpMethod(new BinMethod(if_), 3);
            obj.methods["if"] = new LpMethod(new BinMethod(if_), 3);
            // TODO: sleep
            //obj.methods["while"] = new LpMethod( new BinMethod(print) ); // マクロで再現
            // TODO: until(マクロで再現
            // TODO: p

            // Lv2
            // TODO: callcc
        }

        // TODO: 全く未実装
        private static LpObject alias(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject break_(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.BREAK;
            return LpNl.initialize();
        }

        // 末尾再帰の実装できるかわからないので、便利な道具を優先する
        private static LpObject cond(LpObject self, LpObject[] args, LpObject block = null)
        {
            Func<LpObject, LpObject> fun = (stmt) => (stmt.class_name == "Block" || stmt.class_name == "Lambda") ? stmt.funcall("call", null) : stmt;
            Func<LpObject, bool> test = (stmt) => (bool)fun(stmt).funcall("nil?", null, null).boolValue;
            for (int i = 0; args.Count() >= i + 2; i += 2)
                if (test(args[i]))
                    return fun(args[i + 1]);

            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject exit(LpObject self, LpObject[] args, LpObject block = null)
        {
            Environment.Exit(0);
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject eval(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpParser.PROGRAM.Parse(args[0].stringValue);
        }

        private static LpObject if_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var expr = args[0];
            Func<LpObject, LpObject> fun = (stmt) => (stmt.class_name == "Block" || stmt.class_name == "Lambda") ? stmt.funcall("call", null) : stmt;
            if (expr != null && expr.boolValue != false)
            {
                return fun(args[1]);
            }
            else
            {
                return fun(args[2]);
            }
        }

        // TODO: 全く未実装
        private static LpObject is_block_given(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject load(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject loop(LpObject self, LpObject[] args, LpObject block = null)
        {
            LpObject ret = LpNl.initialize();

            if (block == null) return ret;

            while (true)
            {
                ret = block.funcall("call", args, null);
                // break文
                if (control_status == (int)LpBase.CONTROL_CODE.BREAK)
                {
                    control_status = (int)LpBase.CONTROL_CODE.NONE;
                    break;
                } 
                // next文
                if (control_status == (int)LpBase.CONTROL_CODE.NEXT)
                {
                    control_status = (int)LpBase.CONTROL_CODE.NONE;
                    continue;
                }
            }
            return ret;
        }

        // TODO: 全く未実装
        private static LpObject next_(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.NEXT;
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject return_(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.RETURN;
            return LpNl.initialize();
        }

        private static LpObject print(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return o.funcall("display", null, null);
        }

        // TODO: 全く未実装
        private static LpObject require(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject yield_(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.BREAK;
            return LpNl.initialize();
        }

    }
}
