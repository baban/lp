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
            return args.Aggregate(
                init(),
                (o, stmt) => {
                    var v = LpParser.STMT.Parse(stmt);
                    o.funcall("push", v);
                    return o;
                });
        }

        private static LpObject init()
        {
            var obj = createClassTemplate();
            obj.superclass  = LpObject.initialize();
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
                LpObject obj = LpObject.initialize();
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
            obj.methods["+"] = new BinMethod(concat);
            obj.methods["concat"] = new BinMethod(concat);
            obj.methods["last"] = new BinMethod(last);
            obj.methods["push"] = new BinMethod(push);
            obj.methods["<<"] = new BinMethod(push);
            obj.methods["at"] = new BinMethod(at);
            obj.methods["car"] = new BinMethod(first);
            obj.methods["first"] = new BinMethod(first);
            obj.methods["size"] = new BinMethod(len);
            obj.methods["len"] = new BinMethod(len);
            /*
            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);

            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
             */
            obj.methods["to_s"] = new BinMethod(to_s);
        }

        private static LpObject at( LpObject self, LpObject args )
        {
            var arg = args.arrayValues.First();
            var v = self.arrayValues.ElementAt( (int)arg.doubleValue );
            return v;
        }

        static LpObject first(LpObject self, LpObject args)
        {
            return self.arrayValues.First();
        }

        static LpObject push(LpObject self, LpObject args)
        {
            self.arrayValues.Add(args);
            return self;
        }

        static LpObject to_s(LpObject self, LpObject args)
        {
            var vs = self.arrayValues.Select<LpObject, string>((a, b) => a.funcall("to_s", null).stringValue.ToString() ).ToArray();
            var s = string.Join(", ",vs);
            return LpString.initialize( "["+s+"]" );
        }

        static LpObject len(LpObject self, LpObject args)
        {
            return LpNumeric.initialize( self.arrayValues.Count );
        }

        static LpObject last(LpObject self, LpObject args)
        {
            return self.arrayValues.Last();
        }

        static LpObject concat(LpObject self, LpObject args)
        {
            var v = args.arrayValues.First();
            Console.WriteLine(self.arrayValues.Count);
            self.arrayValues.AddRange( v.arrayValues );
            Console.WriteLine( self.arrayValues.Count );
            return self;
        }
    }
}
