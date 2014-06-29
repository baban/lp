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
    class LpNlTest
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

        private Type initNlModule()
        {
            return getModule("LP.Object.LpNl");
        }

        [Test]
        public void initialize()
        {
            Type ot = initModule();
            Type t = initNlModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual("Nl", o.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void display()
        {
            Type ot = initModule();
            Type t = initNlModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);

            var so = t.GetMethod("display", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual("Nl", so.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void to_s()
        {
            Type ot = initModule();
            Type t = initNlModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            var so = t.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual("", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void inspect()
        {
            Type ot = initModule();
            Type t = initNlModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            var so = t.GetMethod("inspect", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual("nl", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
    }
}
