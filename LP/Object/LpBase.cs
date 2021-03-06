﻿using System.Collections;
using System.Collections.Generic;
using Irony.Interpreter.Ast;

namespace LP.Object
{
    public class LpBase
    {
        public static Dictionary<string, LpObject> classes = new Dictionary<string, LpObject>();

        public enum ControlCode { NONE, RETURN, BREAK, NEXT };
        public ControlCode controlStatus = ControlCode.NONE;

        public LpObject superclass = null;
        public BinMethod method = null;
        public List<BinMethod> methodInfos = null;
        public Util.LpArguments arguments = null;
        public delegate LpObject BinMethod(LpObject self, LpObject[] args, LpObject block = null);
        public Dictionary<string, object> methods = new Dictionary<string, object>();
        public Dictionary<string, object> variables = new Dictionary<string, object>();

        public bool isBinaryClass = false;
        public bool isMethodCached = false;

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
    }
}
