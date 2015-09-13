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
            // Lv0
            // TODO: rescue
            // TODO: redo
            // TODO: retry
            // TODO: raise
            // TODO: abort
            // TODO: self
            // TODO: block_given? // マクロで再現
            obj.methods["break"] = new LpMethod( new BinMethod(break_) );
            // TODO: caller
            obj.methods["exec"] = new LpMethod(new BinMethod(exec), 1);
            obj.methods["eval"] = new LpMethod(new BinMethod(eval), 1);
            obj.methods["exit"] = new LpMethod(new BinMethod(exit), 0);
            obj.methods["cond"] = new LpMethod(new BinMethod(cond), -1);
            obj.methods["loop"] = new LpMethod(new BinMethod(loop), 0);
            obj.methods["next"] = new LpMethod(new BinMethod(next_), -1);
            obj.methods["open"] = new LpMethod(new BinMethod(open), 1);
            obj.methods["return"] = new LpMethod(new BinMethod(return_), -1);
            // TODO: yield // マクロで再現

            // Lv1
            obj.methods["print"] = new LpMethod(new BinMethod(print), 1);
            obj.methods["__if"] = new LpMethod(new BinMethod(if_), 3);
            obj.methods["if"] = new LpMethod(new BinMethod(if_), 3);
            obj.methods["case"] = new LpMethod(new BinMethod(case_), -1);
            obj.methods["__case"] = new LpMethod(new BinMethod(case_), -1);
            obj.methods["load"] = new LpMethod(new BinMethod(load), 1);
            obj.methods["require"] = new LpMethod(new BinMethod(require), 1);
            obj.methods["sleep"] = new LpMethod(new BinMethod(sleep), 1);
            //obj.methods["while"] = new LpMethod( new BinMethod(print) ); // マクロで再現
            // TODO: until(マクロで再現

            // Lv2
            // TODO: callcc
            obj.methods["p"] = new LpMethod(new BinMethod(p_), 1);
            obj.methods["alias"] = new LpMethod(new BinMethod(alias), 2);
        }

        private static LpObject alias(LpObject self, LpObject[] args, LpObject block = null)
        {
            var src = args[0];
            var dst = args[1];

            if (null == self.methods[src.stringValue]) throw new Error.LpNoMethodError();

            self.methods[dst.stringValue] = self.methods[src.stringValue];
            return LpNl.initialize();
        }

        private static LpObject break_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var ret = LpNl.initialize();
            ret.controlStatus = ControlCode.BREAK;
            return ret;
        }

        // 末尾再帰の実装できるかわからないので、便利な道具を優先する
        private static LpObject cond(LpObject self, LpObject[] args, LpObject block = null)
        {
            Func<LpObject, LpObject> fun = (stmt) => (stmt.class_name == "Block" || stmt.class_name == "Lambda") ? stmt.funcall("call", null) : stmt;
            Func<LpObject, bool> test = (stmt) => (bool)fun(stmt).funcall("to_bool", null, null).boolValue;

            var args2 = args.First().arrayValues;

            if (args2.Count() < 1 || (args2.Count() % 2) == 1)
                throw new Error.LpArgumentError();

            for (int i = 0; args2.Count() >= i + 2; i += 2)
                if (test(args2[i]))
                    return fun(args2[i + 1]);

            return LpNl.initialize();
        }

        private static LpObject exit(LpObject self, LpObject[] args, LpObject block = null)
        {
            Environment.Exit(0);
            return LpNl.initialize();
        }

        private static LpObject exec(LpObject self, LpObject[] args, LpObject block = null)
        {
            var expr = args[0];
            return LpNl.initialize();
        }

        private static LpObject eval(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpParser.execute(args[0].stringValue);
        }

        private static LpObject if_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var expr = args[0];
            Func<LpObject, LpObject> fun = (stmt) => (stmt.class_name == "Block" || stmt.class_name == "Lambda") ? stmt.funcall("call", null) : stmt;
            if (expr != null && expr.boolValue != null && expr.boolValue==true)
            {
                return fun(args[1]);
            }
            else
            {
                return fun(args[2]);
            }
        }

        // case function can translate to macro.
        // TODO: case function shoud test that change to macro.
        private static LpObject case_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var caseArgs = args.First().arrayValues;

            if (caseArgs.Count() < 1 )
                throw new Error.LpArgumentError();

            var expr = caseArgs[0];
            for (int i = 1; i < caseArgs.Count(); i += 2) {
                bool ret = (bool)expr.funcall("==", new LpObject[] { caseArgs[i] }, null).boolValue;
                if (ret) {
                    return caseArgs[i + 1].funcall("call", null, null);
                }
            }

            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject is_block_given(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        private static LpObject open(LpObject self, LpObject[] args, LpObject block = null)
        {
            var klass = classes["File"];
            return klass.funcall("open",args,block);
        }

        private static LpObject load(LpObject self, LpObject[] args, LpObject block = null)
        {
            // TODO: load path から使えるものを順番に捜す
            var code = readFile(args[0].stringValue);
            LpParser.execute(code);
            return LpNl.initialize();
        }

        private static LpObject loop(LpObject self, LpObject[] args, LpObject block = null)
        {
            LpObject ret = LpNl.initialize();

            if (block == null) return ret;

            while (true)
            {
                ret = block.funcall("call", args, null);
                // break文
                if (ret.controlStatus == ControlCode.BREAK)
                {
                    ret.controlStatus = ControlCode.NONE;
                    break;
                }
                // next文
                if (ret.controlStatus == ControlCode.NEXT)
                {
                    ret.controlStatus = ControlCode.NONE;
                    break;
                }
            }
            return ret;
        }

        private static LpObject next_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var ret = LpNl.initialize();
            if (args.Count() == 1)
                ret = args[0].arrayValues.First();

            ret.controlStatus = ControlCode.NEXT;
            return ret;
        }

        private static LpObject return_(LpObject self, LpObject[] args, LpObject block = null)
        {
            var ret = LpNl.initialize();
            if( args.Count()==1 )
                ret = args[0].arrayValues.First();

            ret.controlStatus = ControlCode.RETURN;
            return ret;
        }

        private static LpObject print(LpObject self, LpObject[] args, LpObject block = null)
        {
            if (args.Count() <= 0)
                throw new Error.LpArgumentError();

            var o = args[0];
            return o.funcall("display", null, null);
        }

        private static LpObject p_(LpObject self, LpObject[] args, LpObject block = null)
        {
            if (args.Count() <= 0)
                throw new Error.LpArgumentError();

            var o = args[0];
            return o.funcall("inspect",null,null).funcall("display", null, null);
        }

        private static LpObject require(LpObject self, LpObject[] args, LpObject block = null)
        {
            // TODO: load path から使えるものを順番に捜す
            var filename = args[0].stringValue;
            var code = readFile(filename);
            LpParser.execute(code);
            return LpNl.initialize();
        }

        static string readFile(string filename)
        {
            if (!System.IO.File.Exists(filename))
                return null;

            StringBuilder strBuff = new StringBuilder();
            System.IO.StreamReader sr = null;
            sr = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8"));
            while (sr.Peek() >= 0)
            {
                strBuff.Append(sr.ReadLine());
                strBuff.Append("\n");
            }
            sr.Close();
            return strBuff.ToString();
        }

        // TODO: 全く未実装
        private static LpObject yield_(LpObject self, LpObject[] args, LpObject block = null)
        {
            //control_status = (int)CONTROL_CODE.BREAK;
            return LpNl.initialize();
        }

        private static LpObject sleep(LpObject self, LpObject[] args, LpObject block = null)
        {
            var time = args[0].doubleValue;
            System.Threading.Thread.Sleep( (int)time );
            return LpNl.initialize();
        }
    }
}
