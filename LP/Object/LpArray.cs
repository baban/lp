using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpArray : LpObject
    {
        public static LpObject initialize()
        {
            return init();
        }

        private static LpObject init()
        {
            LpObject obj = LpObject.initialize();
            setMethods(obj);
            obj.superclass = LpObject.initialize();
            obj.class_name = "array";
            obj.arrayValues= new List<LpObject>();
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            // TODO: new
            // TODO: []
            // TODO: +
            // TODO: <<
            // TODO: push
            // TODO: car, first
            // TODO: cdr
            // TODO: last
            // TODO: each
            // TODO: map
            // TODO: join
            // TODO: concat
            // TODO: size
            obj.methods["push"] = new BinMethod(push);
            obj.methods["<<"] = new BinMethod(push);
            obj.methods["at"] = new BinMethod(at);
            obj.methods["car"] = new BinMethod(first);
            obj.methods["first"] = new BinMethod(first);
            /*
            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);

            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
             */
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
    }
}
