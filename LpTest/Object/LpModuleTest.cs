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
    public class LpModuleTest
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
            return getModule("LP.LpParser");
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initClassModule()
        {
            return getModule("LP.Object.LpModule");
        }

        [TestMethod]
        public void initialize0()
        {
            Type t = getModule("LP.Object.LpModule");
            var o = new PrivateType(t).InvokeStatic("initialize");
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [TestMethod]
        public void new_()
        {
            Type t = getModule("LP.Object.LpModule");
            var pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize");
            var args = new PrivateType(initParser()).InvokeStatic("parseArgsObject", new object[] { ":Hoge" });
            var so = pt.InvokeStatic("new_", new object[]{ o, args, null });
            Assert.AreEqual("LP.Object.LpObject", so.GetType().ToString());
        }
    }
}
