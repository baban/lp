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
    public class LpArrayTest
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

        private Type initNumericModule()
        {
            return getModule("LP.Object.LpNumeric");
        }

        private Type initArrayModule()
        {
            return getModule("LP.Object.LpArray");
        }

        [TestMethod]
        public void initialize()
        {
            Type ot = initModule();
            Type t = initArrayModule();

            PrivateType pt = new PrivateType(t);
            Assert.IsInstanceOfType(pt.InvokeStatic("initialize"), ot);
        }

        [TestMethod]
        public void push()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var o = initParser().GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[]" });

            var args = new PrivateType(initParser()).InvokeStatic("parseArgsObject", new object[] { "3" });
            var so = new PrivateType(t).InvokeStatic("push", new object[] { o, args, null });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
        }
    }
    /*
    [TestFixture]
    class LpArrayTest
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

        private Type initNumericModule()
        {
            return getModule("LP.Object.LpNumeric");
        }

        private Type initArrayModule()
        {
            return getModule("LP.Object.LpArray");
        }



        [Test]
        public void first()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var o = initParser().GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[3]" });
            var ro = t.GetMethod("first", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual("LP.Object.LpObject", ro.GetType().ToString());
        }

        [Test]
        public void at()
        {
            var p = initParser();
            Type t = initArrayModule();

            // 配列作成
            var o = p.GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[20.0]" });

            // 引数作成
            var args = p.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "0" });

            // メソッド呼び出し
            var ret = t.GetMethod("at", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args, null });
            Assert.AreEqual(20.0, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
        }

        [Test]
        public void len()
        {
            var p = initParser();
            Type ot = initModule();
            Type t = initArrayModule();

            // 配列作成
            var o = p.GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[]" });

            // メソッド呼び出し
            o = t.GetMethod("len", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual(0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void len2()
        {
            var p = initParser();
            Type t = initArrayModule();

            // 配列作成
            var o = p.GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[10]" });

            // メソッド呼び出し
            o = t.GetMethod("len", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual(1, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void last()
        {
            var p = initParser();
            Type t = initArrayModule();

            // 配列作成
            var o = p.GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[10,15,20]" });

            o = t.GetMethod("last", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual(20.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void concat()
        {
            var p = initParser();
            Type t = initArrayModule();

            // 配列作成
            var o = p.GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[1,2,3]" });
            var args = p.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "[4,5,6]" });

            o = t.GetMethod("concat", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args, null });
            o = t.GetMethod("len", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args, null });

            Assert.AreEqual(6.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void to_s()
        {
            var p = initParser();
            Type t = initArrayModule();

            // 配列作成
            var o = p.GetMethod("execute", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { "[1,2,3]" });
            o = t.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });

            Assert.AreEqual("[1, 2, 3]", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
    */
}
