using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    // TODO: メソッドの定義位置を記録するべき
    class LpMethod : LpBase
    {
        static string className = "Method";

        List<System.Reflection.MethodInfo> reflections = null;

        public LpMethod(BinMethod m) { 
            arguments = new Util.LpArguments();
            method = m;
        }

        public LpMethod(BinMethod m, int arity )
        {
            arguments = new Util.LpArguments( arity );
            method = m;
        }

        public LpMethod(List<System.Reflection.MethodInfo> rs)
        {
            arguments = new Util.LpArguments();
            reflections = rs;
        }

        public static LpObject initialize()
        {
            return createClassTemplate();
        }

        public static LpObject initialize(BinMethod method)
        {
            var obj = createClassTemplate();
            obj.method = method;
            return obj;
        }

        public static LpObject initialize( string[] args, List<string> stmts )
        {
            var obj = createClassTemplate();
            obj.arguments = new Util.LpArguments( args );
            //obj.statements = stmts;
            return obj;
        }

        public static LpObject initialize(Util.LpArguments args, List<string> stmts)
        {
            var obj = createClassTemplate();
            obj.arguments = args;
            //obj.statements = stmts;
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

        private static void setMethods(LpObject obj){
            // TODO: comment
            // TODO: arity
            // TODO: inspect
            // TODO: to_s
            // TODO: display
            // TODO: to_block
            // TODO: to_method
            // TODO: to_lambda
            // TODO: call
            //obj.methods["call"] = new LpMethod(new BinMethod(call), -1);
        }

        public LpObject funcall(LpObject self, LpObject[] args, LpObject block = null)
        {
            var dstArgs = arguments.putVariables(args, block);

            if (reflections != null) {
                var arrayTypes = translateArrayTypes(args);
                var convertedArgs = convertArgs(args);
                var info = findMethodInfo(self, arrayTypes);
                object result = info.Invoke(null, convertedArgs);

                return convertReturnValue(result);
            } else {
                return method(self, dstArgs, block);
            }
        }

        System.Reflection.MethodInfo findMethodInfo(LpObject self, string[] arrayTypes)
        {
            var squuezedMethods = reflections;
            var method = squuezedMethods.Find((m) => {
                var parameters = m.GetParameters();
                return (parameters.Length == arrayTypes.Length && isRightParameter(parameters, arrayTypes));
            });
            return method;
        }

        static bool isRightParameter(System.Reflection.ParameterInfo[] parameters, string[] arrayTypes)
        {
            if (parameters.Length != arrayTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.ToString() != arrayTypes[i])
                    return false;
            }

            return true;
        }

        string[] translateArrayTypes(LpObject[] args)
        {
            var typus = args.Select((arg) => {
                switch (arg.class_name)
                {
                    case "String":
                    case "Symbol":
                        return "System.String";
                    case "Numeric":
                        return "System.Int32";
                    default:
                        return "System.Object";
                };
            });
            return typus.ToArray();
        }

        object[] convertArgs(LpObject[] args)
        {
            Func<LpObject, object> f = (o) => {
                switch (o.class_name)
                {
                    case "String":
                    case "Symbol":
                        return o.stringValue;
                    case "Boolean":
                        return o.boolValue;
                    case "Numeric":
                        return o.doubleValue;
                    default:
                        return null;
                }
            };
            return args.Select(f).ToArray();
        }

        LpObject convertReturnValue(object result)
        {
            if (null == result)
                return LpNl.initialize();

            var t = result.GetType();
            switch (t.ToString())
            {
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    return LpNumeric.initialize((int)result);
                case "System.Double":
                    return LpNumeric.initialize((double)result);
                case "System.String":
                case "System.Symbol":
                    return LpString.initialize((string)result);
                default:
                    return LpNl.initialize();
            }
        }
    }

}
