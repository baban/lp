﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace LP.Object
{
    class LpQuote : LpBase
    {
        static string className = "Quote";

        public static LpObject initialize(List<Ast.LpAstNode> nodes)
        {
            return init(nodes);
        }

        private static LpObject init(List<Ast.LpAstNode> nodes)
        {
            LpObject obj = createClassTemplate();
            //obj.statements = nodes;
            return obj;
        }

        private static LpObject init(Ast.LpAstNode node)
        {
            LpObject obj = createClassTemplate();
            //obj.statements = new List<Ast.LpAstNode>(){node};
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

        
        private static void setMethods( LpObject obj )
        {
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["inspect"] = new LpMethod(new BinMethod(inspect), 0);
            obj.methods["display"] = new LpMethod(new BinMethod(display), 0);
        }

        private static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            //return LpString.initialize( self.statements.First().toSource() );
            return null;
        }

        private static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            //return LpString.initialize(string.Format("'{0}", self.statements.First().toSource()));
            return null;
        }

        protected static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine( to_s(self, args, block).stringValue );
            return LpNl.initialize();
        }
    }
}
