using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    // `10 => `10
    // `(10+5) => `10.(+)(5)
    // a = (10+5)
    // `(?(6+a)+5) => `21.(+)(5)
    // `?x => unbound variable x
    // ?実行する => ?文字列化 => マクロに押し込み

    class LpQuasiQuote : LpBase
    {
        static string className = "QuasiQuote";

        public static LpObject initialize()
        {
            return init();
        }

        public static LpObject initialize(List<Ast.LpAstNode> nodes) {
            return init(nodes);
        }

        public static Ast.LpAstNode castAndExpand(Ast.LpAstNode node)
        {
            var leaf = node as Ast.LpAstLeaf;
            if (leaf != null)
            {
                return leaf.DoExpand();
            }

            var questionQuote = node as Ast.LpAstQuestionQuote;
            if (questionQuote != null)
            {
                return questionQuote.DoExpand();
            }

            var methodNode = node as Ast.LpAstMethodCall;
            if (methodNode != null)
            {
                return methodNode.DoExpand();
            }

            return node.DoExpand();
        }

        //`(?recv).times(?*args)
        // `?7 // '7
        // `?a // a
        //  `(a+?b)
        private static LpObject init(List<Ast.LpAstNode> nodes)
        {
            LpObject obj = init();
            obj.statements = nodes.Select((node) => castAndExpand(node) ).ToList();
            return obj;
        }

        private static LpObject init()
        {
            LpObject obj = createClassTemplate();
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
                obj.is_macro = true;
                obj.stringValue = "";
                classes[className] = obj;
                return obj.Clone();
            }
        }

        private static void setMethods(LpObject obj)
        {
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect), 0);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
        }

        private static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(self.statements.First().toSource());
        }

        private static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpString.initialize(string.Format("'{0}", self.statements.First().toSource()));
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine(to_s(self, args, block).stringValue);
            return LpNl.initialize();
        }
    }
}
