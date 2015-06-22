using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace LP.Object
{
    class LpBase
    {
        public static Dictionary<string, LpObject> classes = new Dictionary<string, LpObject>();

        public enum ControlCode { NONE, RETURN, BREAK, NEXT };
        public ControlCode controlStatus = ControlCode.NONE;

        public LpObject superclass = null;
        public BinMethod method = null;
        public Util.LpArguments arguments = null;
        public delegate LpObject BinMethod(LpObject self, LpObject[] args, LpObject block = null);
        public Hashtable methods = new Hashtable();
        public Hashtable variables = new Hashtable();

        public bool? boolValue = null;
        public double? doubleValue = null;
        public string stringValue = null;
        public List<LpObject> arrayValues = null;
        public Dictionary<LpObject, LpObject> hashValues = null;
        public Parser<object[]> parserValue = null;

        public bool is_macro = false;
        public List<Ast.LpAstNode> statements = null;

        public string class_name = null;
    }
}
