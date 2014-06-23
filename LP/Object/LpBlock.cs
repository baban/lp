using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpBlock : LpBase
    {
        static string className = "Block";

        public static LpObject initialize()
        {
            return init();
        }

        static LpObject init()
        {
            var obj = createClassTemplate();
            obj.statements = new List<Ast.LpAstNode>();
            return obj;
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
            obj.statements = stmts.ToList();
            return obj;
        }

        public static LpObject initialize(List<Ast.LpAstNode> stmts, string[] args, bool argsLoose = false )
        {
            LpObject obj = init();
            obj.statements = stmts.ToList();
            obj.arguments = new Util.LpArguments( args, argsLoose );
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
            // TODO: inspect
            obj.methods["to_s"] = new LpMethod( new BinMethod(to_s) );
            obj.methods["display"] = new LpMethod( new BinMethod(display) );
            // TODO: to_block
            // TODO: to_method
            // TODO: to_lambda
            obj.methods["call"] = new LpMethod(new BinMethod(call));
        }

        static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            self.stringValue = self.ToString();
            return self;
        }

        static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            var o = to_s(self, args, null);
            Console.WriteLine(o.stringValue);
            return LpNl.initialize();
        }

        static LpObject call(LpObject self, LpObject[] args, LpObject block = null)
        {
            Util.LpIndexer.push(self);
            var dstArgs = (null == args || args.Count() == 0) ? new LpObject[] { } : args.First().arrayValues.ToArray();
            self.arguments.setVariables(self, dstArgs, block);

            LpObject ret = Object.LpNl.initialize();
            foreach (Ast.LpAstNode stmt in self.statements)
            {
                ret = stmt.Evaluate();
            }
            Util.LpIndexer.pop();
            return ret;
        }
    }
}
