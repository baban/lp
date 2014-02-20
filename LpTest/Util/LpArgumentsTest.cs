using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Util
{
    [TestFixture]
    class LpArgumentsTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Util.LpArguments");
            return t;
        }

        [Test]
        public void arity()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t);
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual( i, 0 );
        }

        [Test]
        public void arity1()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, 1);
        }

        [Test]
        public void arityN()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c", "d" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, 4);
        }

        [Test]
        public void arityAstr()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c", "*d" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, -4);
        }

        [Test]
        public void arityAmp()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t, new object[] { new string[] { "a", "b", "c", "&d" }, false });
            var i = o.GetType().GetMethod("arity").Invoke(o, null);
            Assert.AreEqual(i, 3);
        }
        /*
        [Test]
        public void check()
        {
            Type t = initModule();
            var o = Activator.CreateInstance(t);
            var i = o.GetType().GetMethod("check").Invoke(o, null);
            Assert.AreEqual( true, i );
        }
         */
    }
}
