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
            obj.methods["+"] = new LpMethod( new  BinMethod(concat), 1 );
            obj.methods["concat"] = new LpMethod(new BinMethod(concat), 1);
            obj.methods["last"] = new LpMethod( new BinMethod(last), 0 );
            obj.methods["push"] = new LpMethod(new BinMethod(push), 1);
            obj.methods["<<"] = new LpMethod( new BinMethod(push), 0 );
            obj.methods["at"] = new LpMethod( new BinMethod(at), 1 );
            obj.methods["car"] = new LpMethod(new BinMethod(first), 0);
            obj.methods["first"] = new LpMethod( new BinMethod(first), 0 );
            obj.methods["size"] = new LpMethod( new BinMethod(len), 0 );
            obj.methods["len"] = new LpMethod( new BinMethod(len), 0 );
            obj.methods["map"] = new LpMethod(new BinMethod(map), 0);
            //obj.methods["=="] = new LpMethod( new BinMethod(equal), 1);
            //obj.methods["==="] = new LpMethod( new BinMethod(eq), 1 );

            obj.methods["display"] = new LpMethod( new BinMethod(display), 0);
            obj.methods["inspect"] = new LpMethod( new BinMethod(inspect), 0);
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
        }

        private static LpObject at(LpObject self, LpObject[] args, LpObject block = null)
        {
            var arg = args[0];
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

        static LpObject inspect(LpObject self, LpObject[] args, LpObject block = null)
        {
            var vs = self.arrayValues.Select<LpObject, string>((a, b) => a.funcall("to_s", null).stringValue.ToString()).ToArray();
            var s = string.Join(", ", vs);
            return LpString.initialize("[" + s + "]");
        }

        static LpObject display(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine( to_s(self, args, block).stringValue );
            return LpNl.initialize();
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

        static LpObject map(LpObject self, LpObject[] args, LpObject block = null)
        {
            Console.WriteLine( "map func" );
            Console.WriteLine( block );
            Console.WriteLine( block.class_name );
            return LpArray.initialize(self.arrayValues.Select( (v) => {
                                        Console.WriteLine(v);
                                        Console.WriteLine(v.class_name);
                                        return block.funcall("call", self, new LpObject[] { v }, null);
                                        }).ToArray());
        }
    }
}
