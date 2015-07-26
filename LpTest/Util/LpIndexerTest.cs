using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Util
{
    [TestClass]
    class LpIndexerTest
    {
        private Type getModule(string name)
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType(name);
            return t;
        }

        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Util.LpArguments");
            return t;
        }

        private Type initArgumentsModule()
        {
            return getModule("LP.Object.LpArguments");
        }

        [TestMethod]
        public void set()
        {
            //Type t = getModule("LP.Util.LpIndexer");
            //var types = new Type[] { typeof(string) };
            //var o = t.GetMethod("set", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "bbb" });

        }

        [TestMethod]
        public void get()
        {
        }

        [TestMethod]
        public void push()
        {
            Type t = initParser();
            var p = t.InvokeMember("EXPR", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var arg = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "->() do end" });

            Type m = getModule("LP.Util.LpIndexer");

            m.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });

            var ret = m.GetMethod("push", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { arg });
            Assert.AreEqual("Lambda", ret.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));

            var depth = m.GetMethod("depth", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] {});
            Assert.AreEqual( 1, depth );

            ret = m.GetMethod("push", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { arg });

            depth = m.GetMethod("depth", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Assert.AreEqual(2, depth);
        }

        [TestMethod]
        public void pop()
        {
            Type t = initParser();
            var p = t.InvokeMember("EXPR", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var arg = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "->() do end" });

            Type m = getModule("LP.Util.LpIndexer");

            m.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] {});

            var ret = m.GetMethod("push", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { arg });
            Assert.AreEqual("Lambda", ret.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));

            var depth = m.GetMethod("depth", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Assert.AreEqual(1, depth);

            ret = m.GetMethod("pop", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] {});

            depth = m.GetMethod("depth", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Assert.AreEqual(0, depth);
        }

        [TestMethod]
        public void last()
        {
            Type t = initParser();
            var p = t.InvokeMember("EXPR", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var arg = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "->() do end" });

            Type m = getModule("LP.Util.LpIndexer");
            m.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            var depth = m.GetMethod("depth", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Assert.AreEqual(0, depth);

            var ret = m.GetMethod("last", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Assert.AreEqual("Kernel", ret.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
        }
    }
}
