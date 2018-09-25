using System.Collections.Generic;

namespace LP.Util
{
    class Scope
    {
        static public Dictionary<string, object> findDictionary(Irony.Interpreter.Scope scope, string name) {
            var dic = scope.AsDictionary();
            if (!dic.ContainsKey(name))
            {
                dic[name] = new Dictionary<string, object>();
            }
            return (Dictionary<string, object>)dic[name];
        }

        static public Dictionary<string, object> searchContext(Irony.Interpreter.Scope scope, string type, string name)
        {
            var dic = findDictionary(scope, type);

            if (dic.ContainsKey(name))
            {
                return dic;
            }
            else if (scope.Parent != null)
            {
                return searchContext(scope.Parent, type, name);
            }
            else
            {
                return dic;
            }
        }
    }
}
