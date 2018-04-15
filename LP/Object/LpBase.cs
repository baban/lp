using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Interpreter.Ast;
using System.Reflection;

namespace LP.Object
{
    class LpBase
    {
        public static Dictionary<string, LpObject> classes = new Dictionary<string, LpObject>();

        public enum ControlCode { NONE, RETURN, BREAK, NEXT };
        public ControlCode controlStatus = ControlCode.NONE;

        public LpObject superclass = null;
        public BinMethod method = null;
        public List<BinMethod> methodInfos = null;
        public Util.LpArguments arguments = null;
        public delegate LpObject BinMethod(LpObject self, LpObject[] args, LpObject block = null);
        public Hashtable methods = new Hashtable();
        public Hashtable variables = new Hashtable();

        public bool? boolValue = null;
        public double? doubleValue = null;
        public string stringValue = null;
        public List<LpObject> arrayValues = null;
        public Dictionary<LpObject, LpObject> hashValues = null;
        public System.IO.StreamReader streamReader = null;
        public System.IO.StreamWriter streamWriter = null;

        public bool is_macro = false;
        public AstNode statements = null;

        public string class_name = null;

        private bool TryGetClrMethod(Type type, BindingFlags bindingFlags, bool inherited, bool specialNameOnly,
            string name, string clrNamePrefix, string clrName, string altClrName, out Element.LpMemberInfo method)
        {
            method = null;
            List<Element.OverloadInfo> initialMembers = new List<Element.OverloadInfo>(GetClrMethods(type, bindingFlags, inherited, clrNamePrefix, clrName, altClrName, specialNameOnly));
            if (initialMembers.Count == 0)
            {
                // case [1]
                //
                // Note: This failure might be cached (see CacheFailure) based on the type and name, 
                // therefore it must not depend on any other mutable state:
                method = null;
                return false;
            }
            return true;
        }

        private static IEnumerable<MethodInfo> GetClrMethods(Type type, BindingFlags bindingFlags, bool inherited, string/*!*/ name)
        {
            //return _clrMethodsByName.GetMembers(type, name, inherited).WithBindingFlags(bindingFlags);
            return null;
        }

        private IEnumerable<Element.OverloadInfo> GetClrMethods(Type type, BindingFlags bindingFlags, bool inherited, string prefix, string name, string altName, bool specialNameOnly)
        {
            string memberName = prefix + name;
            string altMemberName = (altName != null) ? prefix + altName : null;
            IEnumerable<MethodInfo> methods = GetClrMethods(type, bindingFlags, inherited, memberName);
            //IEnumerable<MethodInfo> altMethods = (altName != null) ? GetClrMethods(type, bindingFlags, inherited, altMemberName) : Enumerable.Empty<MethodInfo>();
            //foreach (MethodBase method in methods.Concat(altMethods))
            foreach (MethodBase method in methods)
            {
                /*if (IsVisible(method.Attributes, method.DeclaringType, specialNameOnly))
                {
                    yield return new ReflectionOverloadInfo(method);
                }*/
            }
            Type _type = Type.GetType("System.Console");
            _type.GetMethod("WriteLine");
            /*
            if ((bindingFlags & BindingFlags.Instance) != 0)
            {
                var extensions = GetClrExtensionMethods(type, memberName);
                var altExtensions = (altName) != null ? GetClrExtensionMethods(type, altMemberName) : Enumerable.Empty<ExtensionMethodInfo>();

                foreach (var extension in extensions.Concat(altExtensions))
                {
                    // TODO: inherit ExtensionMethodInfo <: OverloadInfo?
                    yield return new ReflectionOverloadInfo(extension.Method);
                }
            }
            */
            return null;
        }
    }
}
