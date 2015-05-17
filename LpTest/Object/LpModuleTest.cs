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
            Type ot = initModule();
            Type t = getModule("LP.Object.LpModule");
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual(0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
}
