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
    class LpStringTest
    {
        private Type getModule(string name) {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType(name);
            return t;
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initStringModule()
        {
            return getModule("LP.Object.LpString");
        }

        private Type initArgumentsModule()
        {
            return getModule("LP.Object.LpArguments");
        }

        private Type initNumericModule()
        {
            return getModule("LP.Object.LpNumeric");
        }

        [Test]
        public void initialize0() {
            Type ot = initModule();
            Type t = initStringModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual( "LP.Object.LpObject", o.GetType().ToString() );
            Assert.AreEqual("", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void initialize1()
        {
            Type ot = initModule();
            Type t = initStringModule();
            var types = new Type[] { typeof(string) };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual("bbb",o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void init()
        {
            Type ot = initModule();
            Type t = initStringModule();
            var o = t.GetMethod("init", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new string[] { "aaa" });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual("aaa", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
        
        [Test]
        public void to_s()
        {
            Type st = initStringModule();
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new string[] { "bbb" });
            var so = st.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]{ o, null, null } );
            Assert.AreEqual("bbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void inspect()
        {
            Type ot = initModule();
            Type st = initStringModule();
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new string[] { "bbb" });
            var so = st.GetMethod("inspect", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual("bbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void display()
        {
            Type ot = initModule();
            Type st = initStringModule();
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new string[] { "bbb" });

            var so = st.GetMethod("display", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.Null(so);
        }

        /*
        [Test]
        public void size()
        {
            Type ot = initModule();
            Type st = initStringModule();
            Type at = initArgumentsModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new object[] { (string)"aaaa" });

            var prms = new object[] { "size", null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(4.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void add()
        {
            Type ot = initModule();
            Type st = initStringModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (string)"aaaa" });

            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { (string)"\"bbbb\"" } });

            var prms = new object[] { "<<", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual("aaaabbbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void plus()
        {
            Type ot = initModule();
            Type st = initStringModule();
            Type at = initArgumentsModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new object[] { (string)"aaaa" });

            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { (string)"\"bbbb\"" } });

            var prms = new object[] { "+", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual("aaaabbbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void eq()
        {
            Type ot = initModule();
            Type st = initStringModule();
            Type at = initArgumentsModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new object[] { (string)"aaaa" });
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { (string)"\"aaaa\"" } });
            var prms = new object[] { "===", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void equal()
        {
            Type ot = initModule();
            Type st = initStringModule();
            Type at = initArgumentsModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new object[] { (string)"aaaa" });

            // 引数
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { (string)"\"aaaa\"" } });

            var prms = new object[] { "==", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
        */
    }
}
