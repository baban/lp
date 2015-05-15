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
    class LpKernelTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initKernel()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpKernel");
            return t;
        }

        [TestMethod]
        public void print()
        {
            Type t = initKernel();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10" });

            var so = new PrivateType(t).InvokeStatic("print", new object[] { null, args, null });
        }
    }
    /*
    [TestFixture]
    class LpKernelTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initKernel()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpKernel");
            return t;
        }

        [Test]
        public void if_()
        {
            Type t = initParser();
            Type k = initKernel();
            var p = t.InvokeMember("ARGS", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var args = t.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { (string)" true, do 10; end, do 20; end " });
            var ret = k.GetMethod("if_", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { null, args, null });
            //Assert.AreEqual(10, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
        }

       [Test]
        public void loop()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "loop() do 10; break(); end" });
        }
    }
*/
}
