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
            obj.methods["break"] = new LpMethod( new BinMethod(_break) );
            // TODO: caller
            // TODO: exec
            obj.methods["eval"] = new LpMethod(new BinMethod(eval));
            obj.methods["exit"] = new LpMethod(new BinMethod(exit));
            obj.methods["if"] = new LpMethod(new BinMethod(_if), 3 );
            obj.methods["loop"] = new LpMethod(new BinMethod(loop), 0 );
            //obj.methods["next"] = new LpMethod( new BinMethod(next_) );
            obj.methods["print"] = new LpMethod( new BinMethod(print), 1 );
            //obj.methods["return"] = new LpMethod( new BinMethod(_return) );
            // TODO: require
            // TODO: sleep
            // TODO: yield // マクロで再現

            // Lv1
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
        private static LpObject load(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject require(LpObject self, LpObject[] args, LpObject block = null)
        {
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

        // TODO: 全く未実装
        private static LpObject is_block_given(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject _yield(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.BREAK;
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject _break(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.BREAK;
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject _next(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.NEXT;
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject _return(LpObject self, LpObject[] args, LpObject block = null)
        {
            control_status = (int)CONTROL_CODE.RETURN;
            return LpNl.initialize();
        }

        // TODO: 全く未実装
        private static LpObject loop(LpObject self, LpObject[] args, LpObject block = null)
        {
            LpObject ret = LpNl.initialize();
            
            if (block != null) return ret;

            while(true){
                ret = block.funcall( "call", null, null );
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
        private static LpObject _while(LpObject self, LpObject[] args, LpObject block = null)
        {
            var expr = args[0];
            while( null != expr ){
              block.funcall( "call", null, null );
            }
            return null;
        }

        private static LpObject print(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = args[0];
            return o.funcall("display", null, null);
        }

        private static LpObject _if(LpObject self, LpObject[] args, LpObject block = null)
        {
            var expr = args[0];
            Func<LpObject, LpObject> fun = (stmt) => (stmt.class_name == "Block" || stmt.class_name == "Lambda") ? stmt.funcall("call", null) : stmt;
            if( expr != null && expr.boolValue != false ){
                return fun(args[1]);
            } else {
                return fun(args[2]);
            }
        }
    }
}
