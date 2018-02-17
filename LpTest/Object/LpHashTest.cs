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
    public class LpHashTest
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

        private Type initNumericModule()
        {
            return getModule("LP.Object.LpNumeric");
        }

        private Type initStringModule()
        {
            return getModule("LP.Object.LpString");
        }

        private Type initHashModule()
        {
            return getModule("LP.Object.LpHash");
        }

        [TestMethod]
        public void initialize()
        {
            Type ot = initModule();
            Type t = initHashModule();
            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        /*
        [TestMethod]
        public void initialize2()
        {
            Type ot = initModule();
            Type t = initHashModule();
            var pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize", new object[] { true });
            //var types = new Type[] { typeof(string[][]) };
            //var prms = new string[][] { };
            //var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { prms });
            //Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [TestMethod]
        public void update()
        {
            Type ot = initModule();
            Type t = initHashModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();

            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);

            var stypes = new Type[] { typeof(double) };
            var arg = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, stypes, null).Invoke(null, new object[] { (double)10 });

            // 引数
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg });

            var prms = new object[] { o, args };
            var so = t.GetMethod("update", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
            var h = so.GetType().InvokeMember("hashValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            Assert.NotNull(h);
        }
        */
    }
}
