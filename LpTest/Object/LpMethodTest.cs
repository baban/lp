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
    class LpMethodTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initStringModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpMethod");
            return t;
        }
        /*
        [Test]
        public void initialize()
        {
            var ot = initModule();
            var t = initStringModule();
            var prms = new object[] { null };
            Assert.AreEqual(t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, prms).GetType().ToString(), "LP.Object.LpObject");
        }
        */
    }
}
