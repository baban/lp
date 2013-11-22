using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    [TestFixture]
    class LpBoolTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initBoolModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpBool");
            return t;
        }

        [Test]
        public void initialize1()
        {
            Type ot = initModule();
            Type t = initBoolModule();
            var types = new Type[] { typeof(bool) };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { true });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual( true, o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void initialize1string()
        {
            Type ot = initModule();
            Type t = initBoolModule();
            var types = new Type[] { typeof(string) };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "true" });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual(true, o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

    }
}
