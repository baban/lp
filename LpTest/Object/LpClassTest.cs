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
    public class LpClassTest
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
            return getModule("LP.Object.LpClass");
        }

        [TestMethod]
        public void initialize()
        {
            Type ot = initModule();
            Type cl = initClassModule();
            var types = new Type[] { typeof(string[]) };
            //var o = cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { new string[] { } });
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
    /*
    [TestFixture]
    class LpClassTest
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
            return getModule("LP.Object.LpClass");
        }

        [Test]
        public void initialize()
        {
            Type ot = initModule();
            Type cl = initClassModule();
            var types = new Type[] { typeof(string[]) };
            var o = cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { new string[]{  } });

            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (bool)true });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });

            var prms = new object[] { "&&", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void new_()
        {
            Type ot = initModule();
            Type cl = initClassModule();
            cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { } });

            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Class.new(:Hoge)" });
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Class.new(:Hoge2) do 10 end" });
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Class.new(:Hoge2) do ->() do 10 end end" });
        }

        [Test]
        public void new_2()
        {
            Type ot = initModule();
            Type cl = initClassModule();
            cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { } });

            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Class.new(:Hoge2) do ->() do 10 end.bind(:hagu) end" });
        }

        [Test]
        public void new_3()
        {
            Type ot = initModule();
            Type cl = initClassModule();
            cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { } });

            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Class.new(:Hoge3) do ->() do 10 end.bind(:hagu) end" });
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Hoge3.new(:Hoge4)" });
            Assert.AreEqual("Hoge4", o.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void new_4()
        {
            Type ot = initModule();
            Type cl = initClassModule();
            cl.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]) }, null).Invoke(null, new object[] { new string[] { } });

            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Class.new(:Hoge3) do print(:aaaa); ->() do 10 end.bind(:hagu) end" });
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Hoge3.new(:Hoge4)" });
            Assert.AreEqual("Hoge4", o.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            //o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "Hoge3.new(:Hoge4).hagu()" });
            //Assert.AreEqual("10", o.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
        */
}
