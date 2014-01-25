using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    [TestFixture]
    class LpClassTest
    {
        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initClassModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpClass");
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
            Type cl = initClassModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(string[]) };
            var o = cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { new string[]{  } });
            /*
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (bool)true });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });

            var prms = new object[] { "&&", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
             */
        }
    }
}
