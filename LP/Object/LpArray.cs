using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpArray : LpBase
    {
        static string className = "Array";

        public static LpObject initialize()
        {
            return init();
        }

        public static LpObject initialize( string[] args )
        {
            var o = init();
            o.arrayValues = args.Select( (stmt) => LpParser.STMT.Parse(stmt) ).ToList();
            return o;
        }

        public static LpObject initialize(LpObject[] args)
        {
            var o = init();
            o.arrayValues = args.ToList();
            return o;
        }

        private static LpObject init()
        {
            var obj = createClassTemplate();
            obj.arrayValues = new List<LpObject>();
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
            // TODO: new
            // TODO: []
            // TODO: cdr
            // TODO: each
            // TODO: map
            // TODO: join
            //obj.methods["+"] = new LpMethod( new  BinMethod(concat) );
            //obj.methods["concat"] = new LpMethod(new BinMethod(concat));
            //obj.methods["last"] = new LpMethod( new BinMethod(last) );
            obj.methods["push"] = new LpMethod(new BinMethod(push));
            /*
            obj.methods["<<"] = new LpMethod( new BinMethod(push) );
            obj.methods["at"] = new LpMethod( new BinMethod(at) );
            obj.methods["car"] = new LpMethod(new BinMethod(first));
            obj.methods["first"] = new LpMethod( new BinMethod(first) );
            obj.methods["size"] = new LpMethod( new BinMethod(len) );
            obj.methods["len"] = new LpMethod( new BinMethod(len) );
             */
            /*
            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);

            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
             */
            //obj.methods["to_s"] = new LpMethod(new BinMethod(to_s));
        }

        private static LpObject at(LpObject self, LpObject args, LpObject block = null)
        {
            var arg = args.arrayValues.First();
            var v = self.arrayValues.ElementAt( (int)arg.doubleValue );
            return v;
        }

        static LpObject first(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self.arrayValues.First();
        }

        static LpObject push(LpObject self, LpObject[] args, LpObject block = null)
        {
            self.arrayValues.Add(args[0]);
            return self;
        }

        static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            var vs = self.arrayValues.Select<LpObject, string>((a, b) => a.funcall("to_s", null).stringValue.ToString() ).ToArray();
            var s = string.Join(", ",vs);
            return LpString.initialize( "["+s+"]" );
        }

        static LpObject len(LpObject self, LpObject[] args, LpObject block = null)
        {
            return LpNumeric.initialize( self.arrayValues.Count );
        }

        static LpObject last(LpObject self, LpObject[] args, LpObject block = null)
        {
            return self.arrayValues.Last();
        }

        static LpObject concat(LpObject self, LpObject[] args, LpObject block = null)
        {
            var v = args[0];
            self.arrayValues.AddRange( v.arrayValues );
            return self;
        }
    }
}
