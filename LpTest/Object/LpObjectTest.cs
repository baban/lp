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
    public class LpObjectTest
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

        [TestMethod]
        public void initialize()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            Assert.IsInstanceOfType(pt.InvokeStatic("initialize"),t);
        }

        [TestMethod]
        public void to_s()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize");
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual( str, null );
            
            // 引数なし
            var so = pt.InvokeStatic("to_s", new object[] { o, null, null });
            str = so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.IsTrue(r.IsMatch(str.ToString()));
        }

        [TestMethod]
        public void funcall()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize");

            // 引数なし
            var so = new PrivateObject(o).Invoke("funcall", new object[] { "to_s", null, null });
            var str = so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.IsTrue(r.IsMatch(str.ToString()));
        }

        [TestMethod]
        public void funcall_no_method_raise()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize");

            // 引数なし
            try {
                pt.InvokeStatic("funcall", new object[] { "to_s_error", null });
            }
            catch
            {
                return;
            }
            Assert.Fail("We should not get here");
        }

        [TestMethod]
        public void is_nil()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize");

            // 引数なし
            var so = pt.InvokeStatic("is_nil", new object[] { o, null, null });
            var b = so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            Assert.IsFalse( (bool)b );
        }

        [TestMethod]
        public void define_method()
        {
            var pm = initParser();
            var p = pm.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, pm, null);

            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "\"hoge\"" });
            /*
            var block = pm.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" do 10; end " });

            // 引数なし
            var prms = new object[] { o, args, block };
            var so = t.GetMethod("define_method", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
            */
        }

        [TestMethod]
        public void send()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            var o = pt.InvokeStatic("initialize");

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "\"to_s\"" });

            // 引数なし
            var so = pt.InvokeStatic("send", new object[] { o, args, null });
        }

        [TestMethod]
        public void alias()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "\"to_s\",\"hoge\"" });

            var prms = new object[] { o, args, null };
            var so = t.GetMethod("alias", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
        }

        [TestMethod]
        public void methods()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "" });

            var prms = new object[] { o, args, null };
            var so = t.GetMethod("methods_", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
        }

        [TestMethod]
        public void instance_eval()
        {
            var pm = initParser();
            var p = pm.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, pm, null);
            /*
            var block = pm.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" do 10; end " });

            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "" });

            var prms = new object[] { o, args, block };
            var so = t.GetMethod("instance_eval", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
            Assert.AreEqual(10, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
             */
        }

        [TestMethod]
        public void is_a()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            /*
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10" });

            var prms = new object[] { o, args, null };
            var so = t.GetMethod("is_a", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
            Assert.AreEqual(false, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
             */
        }

        [TestMethod]
        public void extend()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
            // 未実装
            //var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "class Hoge" });

            //var prms = new object[] { o, args, null };
            //var so = t.GetMethod("extend", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
            //Assert.AreEqual(false, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
    }
}
