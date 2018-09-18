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
    }
}
