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
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initStringModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpString");
            return t;
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
            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });
            var so = st.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]{ o, null } );
            Assert.AreEqual("bbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void inspect()
        {
            Type ot = initModule();
            Type st = initStringModule();
            var types = new Type[] { typeof(string) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });
            var so = st.GetMethod("inspect", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual("bbb", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

    }
}
