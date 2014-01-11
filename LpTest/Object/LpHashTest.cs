using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    [TestFixture]
    class LpHashTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initNumericModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpNumeric");
            return t;
        }

        private Type initStringModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpString");
            return t;
        }

        private Type initHashModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpHash");
            return t;
        }

        private Type initArgumentsModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpArguments");
            return t;
        }

        [Test]
        public void initialize()
        {
            Type ot = initModule();
            Type t = initHashModule();
            var types = new Type[] { };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }

        [Test]
        public void update()
        {
            Type ot = initModule();
            Type t = initHashModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();

            var types = new Type[] { };
            var o = t.GetMethod("initialize").Invoke(null,null);

            var stypes = new Type[] { typeof(double) };
            var arg = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, stypes, null).Invoke(null, new object[] { (double)10 });

            // 引数
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg });

            var prms = new object[] { o,args };
            var so = t.GetMethod("update", BindingFlags.Static | BindingFlags.NonPublic ).Invoke(o,prms);
            var h = so.GetType().InvokeMember("hashValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            Assert.NotNull(h);
        }
    }
}
