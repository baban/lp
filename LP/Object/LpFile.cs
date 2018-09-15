using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    public class LpFile : LpBase
    {
        static string className = "File";

        public static LpObject initialize()
        {
            return init();
        }

        private static LpObject init()
        {
            LpObject obj = createClassTemplate();
            return obj;
        }

        private static void setMethods(LpObject obj)
        {
            // TODO: new
            // TODO: <<
            obj.methods["open"] = new LpMethod(new BinMethod(open), 2);
            obj.methods["read"] = new LpMethod(new BinMethod(read), 0);
            obj.methods["write"] = new LpMethod(new BinMethod(write), 1);
            obj.methods["to_s"] = new LpMethod(new BinMethod(to_s), 0);
            obj.methods["close"] = new LpMethod(new BinMethod(close), 0);
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

        private static LpObject open(LpObject self, LpObject[] args, LpObject block = null)
        {
            var filename = args[0].stringValue;
            var mode = args[1].stringValue;
            var obj = self.Clone();
            obj.stringValue = mode;
            switch (mode) {
                case "r":
                case "r+":
                    obj.streamReader = new System.IO.StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8"));
                    break;
                case "w":
                case "w+":
                    obj.streamWriter = new System.IO.StreamWriter(filename);
                    break;
                default:
                    throw new Error.LpArgumentError();
            }

            return obj;
        }

        private static LpObject read(LpObject self, LpObject[] args, LpObject block = null)
        {
            if( self.streamReader==null )
                throw new Error.LpIOError();

            string line = self.streamReader.ReadLine();
            return Object.LpString.initialize(line);
        }

        private static LpObject write(LpObject self, LpObject[] args, LpObject block = null)
        {
            if (self.streamWriter == null)
                throw new Error.LpIOError();

            var text = args[0].stringValue;
            self.streamWriter.Write(text);
            return Object.LpNumeric.initialize(text.Length);
        }

        private static LpObject close(LpObject self, LpObject[] args, LpObject block = null)
        {
            switch (self.stringValue) {
                case "w":
                case "w+":
                    if (self.streamWriter != null)
                    {
                        self.streamWriter.Close();
                        self.streamWriter = null;
                    }
                    break;
                case "r":
                case "r+":
                    if (self.streamReader != null)
                    {
                        self.streamReader.Close();
                        self.streamReader = null;
                    }
                    break;
            }
            return LpNl.initialize();
        }

        protected static LpObject to_s(LpObject self, LpObject[] args, LpObject block = null)
        {
            var str = string.Format("#<{0}:{0:x8}>", className, self.GetHashCode());
            return LpString.initialize(str);
        }
    }
}
