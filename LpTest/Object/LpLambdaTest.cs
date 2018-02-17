using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    [TestClass]
    public class LpLambdaTest
    {
        private Type getModule(string name)
        {
            return Assembly.LoadFrom("LP.exe").GetModule("LP.exe").GetType(name);
        }

        private Type initParser()
        {
            return getModule("LP.LpParser");
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initSymbolModule()
        {
            return getModule("LP.Object.LpSymbol");
        }

        private Type initLambdaModule()
        {
            return getModule("LP.Object.LpLambda");
        }

        [TestMethod]
        public void initialize()
        {
            Type t = initLambdaModule();

            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, new object[] { });
            Assert.AreEqual(o.GetType().ToString(), "LP.Object.LpObject");
        }
    }
    /*
    [TestFixture]
    class LpLambdaTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type getModule(string name)
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType(name);
            return t;
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initSymbolModule()
        {
            return getModule("LP.Object.LpSymbol");
        }

        private Type initLambdaModule()
        {
            return getModule("LP.Object.LpLambda");
        }

        [Test]
        public void call()
        {
            Type t = initParser();
            var p = t.InvokeMember("LAMBDA", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = " ->() do 10; end ";
            var o = t.GetMethod("parseToObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var prms = new object[] { "call", o, null, null };
            // 引数なし
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(10.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void call2()
        {
            Type t = initParser();
            var p = t.InvokeMember("LAMBDA", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = " ->(g) do g; end ";
            var o = t.GetMethod("parseToObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "15" });
            var prms = new object[] { "call", o, args, null };
            // 引数なし
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(15.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void bind()
        {
            Type p = initParser();
            var block = "->() do end.bind(:aaa)";
            var o = initParser().GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "->() do end" });
            Assert.AreEqual(o.GetType().ToString(), "LP.Object.LpObject");
        }
    }
     * */
}


