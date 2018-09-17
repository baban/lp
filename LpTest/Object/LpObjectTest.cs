using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    [TestClass]
    public class LpObjectTest
    {
        private Type initParser()
        {
            return TestHelper.InitParser();
        }

        private Type initModule()
        {
            return TestHelper.getModule("LP.Object.LpObject");
        }

        [TestMethod]
        public void initialize()
        {
            Type t = initModule();
            PrivateType pt = new PrivateType(t);
            Assert.IsInstanceOfType(pt.InvokeStatic("initialize"), t);
        }

        [TestMethod]
        public void to_s()
        {
            // 引数なし
            var str = LP.Object.LpObject.initialize().funcall("to_s", null, null).stringValue;

            var r = new Regex(@"<obj \w+?>");
            Assert.IsTrue(r.IsMatch(str.ToString()));
        }

        [TestMethod]
        public void funcall_no_method_raise()
        {
            try {
                var str = LP.Object.LpObject.initialize().funcall("hoge", null, null).stringValue;
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
            // 引数なし
            var v = LP.Object.LpObject.initialize().funcall("nil?", null, null).boolValue;
            Assert.AreEqual(false, v);
        }
        
        [TestMethod]
        public void define_method()
        {
            // 引数なし
            var o = LP.Object.LpObject.initialize();
            var args = new LP.Object.LpObject[]{ LP.Object.LpString.initialize("hoge") };
            var ret = o.funcall("define_method", args, LP.Object.LpBlock.initialize() );

            Assert.AreEqual("hoge", ret.stringValue);
        }

        /*
        [TestMethod]
        public void send()
        {
            // 引数なし
            var o = LP.Object.LpObject.initialize();

            var ret = o.funcall("send", new LP.Object.LpObject[] { LP.Object.LpString.initialize("to_s") }, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.IsTrue(r.IsMatch(ret.ToString()));
        }
        */

        [TestMethod]
        public void alias()
        {
            // 引数なし
            var o = LP.Object.LpObject.initialize();
            var args = new LP.Object.LpObject[] { LP.Object.LpString.initialize("to_s"), LP.Object.LpString.initialize("hoge") };
            var v = o.funcall("alias", args, null);
            Assert.AreEqual("hoge", v.stringValue);
        }

        /*
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
        }

        [TestMethod]
        public void is_a()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10" });

            var prms = new object[] { o, args, null };
            var so = t.GetMethod("is_a", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
            Assert.AreEqual(false, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
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
        */
    }
}
