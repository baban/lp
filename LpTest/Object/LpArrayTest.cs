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
        public void initialize()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var types = new Type[] {};
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [Test]
        public void initialize2()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var types = new Type[] { typeof(string[]) };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[]{ new string[]{ "10", "5" } });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [Test]
        public void push()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, null);
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "3" });

            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
        }

        [Test]
        public void first()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, null);
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "3" });

            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);

            var so2 = t.GetMethod("first", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { so, null });
            Assert.AreEqual("LP.Object.LpObject", so2.GetType().ToString());
        }

        /*
        [Test]
        public void at()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type at = initArgumentsModule();
            Type t = initArrayModule();

            // 配列作成
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { "20.0" } });

            // 引数作成
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[]{ new string[]{ "0.0" } } );

            // メソッド呼び出し
            var so2 = t.GetMethod("at", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args });
            Assert.AreEqual(20.0, so2.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so2, null));
        }

        [Test]
        public void len()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type at = initArgumentsModule();
            Type t = initArrayModule();

            // 配列作成
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, null);

            // メソッド呼び出し
            o = t.GetMethod("len", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual(0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { "10" } });
            o = t.GetMethod("len", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual(1, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void last()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type at = initArgumentsModule();
            Type t = initArrayModule();

            // 配列作成
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { "10", "15", "20" } });

            o = t.GetMethod("last", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual(20.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void concat()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type at = initArgumentsModule();
            Type t = initArrayModule();

            // 配列作成
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { "1","2","3" } });

            var args = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { "[4,5,6]" } });

            o = t.GetMethod("concat", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args });
            o = t.GetMethod("len", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, args });

            Assert.AreEqual(6.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void to_s()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type at = initArgumentsModule();
            Type t = initArrayModule();

            // 配列作成
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { "1", "2", "3" } });

            o = t.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });

            Assert.AreEqual("[1, 2, 3]", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
        */
    }
}
