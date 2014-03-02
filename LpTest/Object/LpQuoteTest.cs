using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;

namespace LpTest.Object
{
    [TestFixture]
    class LpQuoteTest
    {
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

        [Test]
        public void initialize1()
        {
            Type ot = initModule();
            Type t = getModule("LP.Object.LpQuote");
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null).Invoke(null, new string[] { "bbb" });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual("bbb", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
}
