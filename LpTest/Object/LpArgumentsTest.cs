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
    class LpArgumentsTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
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
        public void initialize0()
        {
            Type ot = initModule();
            Type t = initArgumentsModule();
            var types = new Type[] {};
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [Test]
        public void push()
        {
            Type ot = initModule();
            Type t = initArgumentsModule();
            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);

            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, o });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
        }

        [Test]
        public void first()
        {
            Type ot = initModule();
            Type t = initArgumentsModule();
            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);

            var so = t.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, o });
            var ary = so.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var so2 = t.GetMethod("first", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { so, so });
            Assert.AreEqual("LP.Object.LpObject", so2.GetType().ToString());
        }
    }
}
