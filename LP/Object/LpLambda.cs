using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using System.Diagnostics;

namespace LP.Object
{
    class LpLambda : LpBase
    {
        static string className = "Lambda";

        public static LpObject initialize()
        {
            return init();
        }

        public static LpObject initialize(Ast.LpAstNode stmt)
        {
            LpObject obj = init();
            obj.statements.Add(stmt);
            return obj;
        }

        public static LpObject initialize(List<Ast.LpAstNode> stmts)
        {
            LpObject obj = init();
            obj.statements = stmts;
            return obj;
        }

        public static LpObject initialize(List<Ast.LpAstNode> stmts, string[] args, bool argLoose = false )
        {
            LpObject obj = init();
            obj.statements = stmts;
            obj.arguments = new Util.LpArguments(args, argLoose);
            obj.superclass = LpObject.initialize();
            return obj;
        }

        static LpObject init()
        {
            LpObject obj = createClassTemplate();
            obj.statements = new List<Ast.LpAstNode>();
            return obj;
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
                obj.superclass = LpObject.initialize();
                obj.class_name = className;
                classes[className] = obj;
                return obj.Clone();
            }
        }

        private static void setMethods(LpObject obj)
        {
            // TODO: display
            // TODO: inspect
            // TODO: to_s
            // TODO: to_block
            // TODO: to_method
            // TODO: to_lambda
            //obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            //obj.methods["display"] = new LpMethod(new BinMethod(display));
            obj.methods["call"] = new LpMethod(new BinMethod(call),-1);
            obj.methods["bind"] = new LpMethod(new BinMethod(bind), 1);
            obj.methods["to_method"] = new LpMethod(new BinMethod(to_method), 0);
            obj.methods["to_class"] = new LpMethod(new BinMethod(to_class), 1);
        }

        static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            self.stringValue = self.ToString();
            return self;
        }

        static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var so = to_s(self, args);
            Console.WriteLine(so.stringValue);
            return Object.LpNl.initialize();
        }

        public static LpObject call(LpObject self, LpObject[] args, LpObject block = null)
        {
            Util.LpIndexer.push(self);
            var dstArgs = (null == args) ? new LpObject[] { } : args;
            self.arguments.setVariables(self, dstArgs, block);

            LpObject ret = Object.LpNl.initialize();
            foreach (Ast.LpAstNode stmt in self.statements)
            {
                ret = stmt.Evaluate();

                if (ret.controlStatus == LpBase.ControlCode.RETURN) break;
            }
            Util.LpIndexer.pop();
            return ret;
        }

        static LpObject to_class(LpObject self, LpObject[] args, LpObject block = null)
        {
            var name = args[0].stringValue;
            return LpClass.initialize( name, self.statements.ToList() );
        }
         
        static LpObject to_method(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpMethod.initialize(self.arguments, self.statements.ToList() );
        }

        static Object.LpObject bind(LpObject self, LpObject[] args, LpObject block = null)
        {
            var name = args[0].stringValue;
            var ctx =Util.LpIndexer.getLatestClass();
            ctx.methods[name] = self;
            return self;
        }
    }
}
