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
    public class LpBlockTest
    {
        private Type getModule(string name)
        {
            return Assembly.LoadFrom("LP.exe").GetModule("LP.exe").GetType(name);
        }

        private Type initParser()
        {
            return getModule("LP.LpParser");
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initBlockModule()
        {
            return getModule("LP.Object.LpBlock");
        }

        [TestMethod]
        public void initialize()
        {
            Type t = initBlockModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }
    }
    /*
    [TestFixture]
    class LpBlockTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

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

        private Type initBlockModule()
        {
            return getModule("LP.Object.LpBlock");
        }

        [Test]
        public void initialize()
        {
            Type t = initBlockModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
        }
     
        [Test]
        public void to_s()
        {
            Type t = initBlockModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);

            // 引数なし
            var prms = new object[] { o, null, null };
            var so = t.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, prms);
            var str = so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.True(r.IsMatch(str.ToString()));
        }

        [Test]
        public void display()
        {
            Type t = initBlockModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);

            // 引数なし
            var prms = new object[] { o, null, null };
            var so = t.GetMethod("display", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, prms);
        }

        [Test]
        public void call()
        {
            Type t = initParser();
            var p = t.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = " do 10; end ";
            var o = t.GetMethod("parseToObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });

            // 引数なし
            var prms = new object[] { "call", null, null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(10.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
    }
    */
}
