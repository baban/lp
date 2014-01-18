using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP.Object
{
    class LpBase
    {
        protected static Dictionary<string, LpObject> classes = new Dictionary<string, LpObject>();

        public LpObject superclass = null;
        public BinMethod method = null;
        public delegate LpObject BinMethod(LpObject self, LpObject args);
        public Hashtable methods = new Hashtable();
        public Hashtable variables = new Hashtable();

        public bool? boolValue = null;
        public double? doubleValue = null;
        public string stringValue = null;
        public List<LpObject> arrayValues = null;
        public HashSet<LpObject> hashValues = null;
        public List<string> statements = null;

        public string class_name = null;
    }
}
