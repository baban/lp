using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Util
{
    [TestClass]
    class LpArgumentsTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Util.LpArguments");
            return t;
        }

        [TestMethod]
        public void arity()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t);
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual( i, 0 );
        }

        [TestMethod]
        public void arity1()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, 1);
        }
        /*
        [TestMethod]
        public void arityN()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c", "d" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, 4);
        }

        [TestMethod]
        public void arityAstr()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c", "*d" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, -4);
        }

        [TestMethod]
        public void arityAmp()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c", "&d" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, 3);
        }

        [TestMethod]
        public void check()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "" });

            var o = Activator.CreateInstance(t);
            var prms = new object[] { args };
            var i = o.GetType().GetMethod("check").Invoke(o, prms);
            Assert.AreEqual( true, i );
        }

        [TestMethod]
        public void check1()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10" });

            var o = Activator.CreateInstance(t, new object[] { new string[] { "a" }, false });
            var prms = new object[] { args };
            var i = o.GetType().GetMethod("check").Invoke(o, prms);
            Assert.AreEqual(true, i);
        }

        [TestMethod]
        public void checkN()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10, 20, 30" });

            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c" }, false });
            var prms = new object[] { args };
            var i = o.GetType().GetMethod("check").Invoke(o, prms);
            Assert.AreEqual(true, i);
        }

        [TestMethod]
        public void checkN2()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10, 20, 30" });

            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "*c" }, false });
            var prms = new object[] { args };
            var i = o.GetType().GetMethod("check").Invoke(o, prms);
            Assert.AreEqual(true, i);
        }

        [TestMethod]
        public void checkN3()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10, 20, 30, 40" });

            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "*c" }, false });
            var prms = new object[] { args };
            var i = o.GetType().GetMethod("check").Invoke(o, prms);
            Assert.AreEqual(true, i);
        }

        [TestMethod]
        public void checkN4()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10, 20" });

            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "*c" }, false });
            var prms = new object[] { args };
            var i = o.GetType().GetMethod("check").Invoke(o, prms);
            Assert.AreEqual(false, i);
        }

        [TestMethod]
        public void putVariables()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10" });

            var o = Activator.CreateInstance(t, new object[] { new string[] { "a" }, false });
            var prms = new object[] { args, null };
            var ret = o.GetType().GetMethod("putVariables").Invoke(o, prms);
            Assert.NotNull( ret );
        }

        [TestMethod]
        public void putVariables2()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10, 20" });

            var o = Activator.CreateInstance(t, new object[] { 2 });
            var prms = new object[] { args, null };
            var ret = o.GetType().GetMethod("putVariables").Invoke(o, prms);
            Assert.NotNull(ret);
        }

        [TestMethod]
        public void putVariables3()
        {
            Type t = initModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10, 20" });

            var o = Activator.CreateInstance(t, new object[] { -1 });
            var prms = new object[] { args, null };
            var ret = o.GetType().GetMethod("putVariables").Invoke(o, prms);
            Assert.NotNull(ret);
        }
        */
    }
}
