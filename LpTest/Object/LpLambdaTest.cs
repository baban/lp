using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
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
        public void initialize()
        {
            Type t = initLambdaModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] {  }, null).Invoke(null, new object[] {  });
            Assert.AreEqual( o.GetType().ToString(), "LP.Object.LpObject");
        }

        [Test]
        public void call()
        {
            Type t = initParser();
            var p = t.InvokeMember("LAMBDA", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = " ->() do 10; end ";
            var node = t.GetMethod("parseNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var o = node.GetType().InvokeMember("DoEvaluate", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, node, null);
            var prms = new object[] { "call", o, null, null };
            // 引数なし
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(10.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void bind()
        {
            Type t = initLambdaModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, new object[] { });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { ":aaa" });
            var prms = new object[] { o, args, null };
            var so = t.GetMethod("bind", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, prms);
            Assert.AreEqual(so.GetType().ToString(), "LP.Object.LpObject");
        }
    }
}
