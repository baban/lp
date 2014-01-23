using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpHash : LpBase
    {
        static string className = "Hash";

        public static LpObject initialize()
        {
            return init();
        }

        private static LpObject init()
        {
            LpObject obj = createClassTemplate();
            obj.hashValues = new HashSet<LpObject>();
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            // TODO: new
            // TODO: []
            // TODO: each
            // TODO: map
            // TODO: keys
            // TODO: values
            // TODO: size
            obj.methods["update"] = new LpMethod( new BinMethod(update) );
            obj.methods["size"] = new LpMethod( new BinMethod(size));
            // TODO: to_a
            /*
            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);

            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
             */
        }

        private static LpObject size(LpObject self, LpObject args)
        {
            return LpNumeric.initialize(self.hashValues.Count);
        }

        private static LpObject update(LpObject self, LpObject args)
        {
            var k = args.arrayValues[0];
            //var v = args.arrayValues[1];
            self.hashValues.Add(k);
            return self;
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
    }
}
