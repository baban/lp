using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LpTest.Object
{
    [TestClass]
    public class LpQuoteTest
    {
        private Type getModule(string name)
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType(name);
            return t;
        }
        /*
        [TestMethod]
        public void initialize1()
        {
            Type ot = getModule("LP.Object.LpObject");
            Type t = getModule("LP.Object.LpQuote");
            var pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize", new object[] { "bbb" });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual("bbb", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
         */
    }
}
