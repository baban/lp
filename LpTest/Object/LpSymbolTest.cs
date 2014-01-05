using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;


namespace LpTest.Object
{
    [TestFixture]
    class LpSymbolTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initSymbolModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpSymbol");
            return t;
        }

        private Type initArgumentsModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpArguments");
            return t;
        }

        [Test]
        public void initialize1()
        {
            Type ot = initModule();
            Type t = initSymbolModule();
            var types = new Type[] { typeof(string) };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual("bbb", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void to_s()
        {
            Type st = initSymbolModule();
            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });
            var so = st.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual("bbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void inspect()
        {
            Type ot = initModule();
            Type st = initSymbolModule();
            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });

            var so = st.GetMethod("inspect", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual("bbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void eq()
        {
            Type ot = initModule();
            Type st = initSymbolModule();
            Type at = initArgumentsModule();

            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (string)"aaaa" });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (string)"aaaa" });
            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { "===", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void equal()
        {
            Type ot = initModule();
            Type st = initSymbolModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (string)"aaaa" });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (string)"aaaa" });
            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { "==", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
    }
}
