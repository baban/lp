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
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initNumericModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpNumeric");
            return t;
        }

        private Type initArrayModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpArray");
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
        public void initialize()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var types = new Type[] {};
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [Test]
        public void push()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);

            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, o });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
        }

        [Test]
        public void first()
        {
            Type ot = initModule();
            Type t = initArrayModule();
            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);

            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, o });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);

            var so2 = t.GetMethod("first", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { so, so });
            Assert.AreEqual("LP.Object.LpObject", so2.GetType().ToString());
        }

        [Test]
        public void at()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type at = initArgumentsModule();
            Type t = initArrayModule();
            var types = new Type[] { };
            var types2 = new Type[] { typeof(double) };

            // 配列作成
            var val = nt.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types2, null).Invoke(null, new object[] { (double)20.0 });
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);
            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, val });

            // 引数作成
            var arg = nt.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types2, null).Invoke(null, new object[] { (double)0.0 });
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg });

            // メソッド呼び出し
            var so2 = t.GetMethod("at", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { so, args });
            Assert.AreEqual(20.0, so2.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so2, null));
        }
    }
}
