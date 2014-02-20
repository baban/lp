using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    // Lv1
    class LpHash : LpBase
    {
        static string className = "Hash";

        public static LpObject initialize()
        {
            return init();
        }

        public static LpObject initialize( string[][] pairs )
        {
            var obj = init();
            foreach (var pair in pairs)
            {
                obj.hashValues[LpParser.STMT.Parse(pair[0])] = LpParser.STMT.Parse(pair[1]);
            }
            return obj;
        }

        private static LpObject init()
        {
            LpObject obj = createClassTemplate();
            obj.hashValues = new Dictionary<LpObject, LpObject>();
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
            // TODO: len
            //obj.methods["update"] = new LpMethod( new BinMethod(update) );
            //obj.methods["len"] = new LpMethod( new BinMethod(len));
            // TODO: to_a
            /*
            obj.methods["=="] = new BinMethod(equal);
            obj.methods["==="] = new BinMethod(eq);

            obj.methods["to_s"] = new BinMethod(to_s);
            obj.methods["display"] = new BinMethod(display);
            obj.methods["inspect"] = new BinMethod(inspect);
             */
        }

        private static LpObject len(LpObject self, LpObject args)
        {
            return LpNumeric.initialize(self.hashValues.Count);
        }

        private static LpObject update(LpObject self, LpObject args)
        {
            var k = args.arrayValues[0];
            //var v = args.arrayValues[1];
            //self.hashValues.Add(k);
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
