using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LpTest.Object
{
    [TestClass]
    class LpNumericTest
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

        private Type initNumericModule()
        {
            return getModule("LP.Object.LpNumeric");
        }

        [TestMethod]
        public void initialize0()
        {
            Type ot = initModule();
            Type t = initNumericModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual(0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        /*

            [Test]
            public void initialize1double()
            {
                Type ot = initModule();
                Type t = initNumericModule();
                var types = new Type[] { typeof(double) };
                var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
                Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
                Assert.AreEqual( 1.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            }

            [Test]
            public void initialize1int()
            {
                Type ot = initModule();
                Type t = initNumericModule();
                var types = new Type[] { typeof(int) };
                var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (int)1 });
                Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
                Assert.AreEqual(1.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            }

            [Test]
            public void initialize1string()
            {
                Type ot = initModule();
                Type t = initNumericModule();
                var types = new Type[] { typeof(string) };
                var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { "1" });
                Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
                Assert.AreEqual(1.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
                o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new string[] { " 2.5 " });
                Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
                Assert.AreEqual(2.5, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            }

            [Test]
            public void to_s()
            {
                Type ot = initModule();
                Type st = initNumericModule();
                var types = new Type[] { typeof(double) };
                var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
                var so = st.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
                Assert.AreEqual("1", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
            }

            [Test]
            public void to_s_funcall()
            {
                Type ot = initModule();
                Type st = initNumericModule();
                var types = new Type[] { typeof(double) };
                var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.5 });
                var prms = new object[] { "to_s", o, null, null };
                var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
                Assert.AreEqual("1.5", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
            }
         * */
        /*
        [Test]
        public void class_funcall()
        {
            Type ot = initModule();
            Type nt = initNumericModule(); 
            var types = new Type[] { typeof(double) };
            var o = nt.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.5 });

            var prms = new object[] { "class", null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual("Numeric", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
        */
        /*
        [Test]
        public void hash_funcall()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = nt.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.5 });

            var prms = new object[] { "hash", null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            var v = so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            Assert.NotNull(v);
        }
        */
        /*
        [Test]
        public void display()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
            var so = st.GetMethod("display", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.IsNotNull(so);
        }

        [Test]
        public void inspect()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
            var so = st.GetMethod("inspect", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null, null });
            Assert.AreEqual("1", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void to_i()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.5 });

            // 引数なし
            var prms = new object[] { "to_i", o, null, null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(10.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void plus()
        {
            Type ot = initModule();
            Type st = initNumericModule();

            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "15" });
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(double) }, null).Invoke(null, new object[] { (double)10.0 });
            var prms = new object[] { "+", o, args, null };
            var ret = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(25.0, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
        }

        [Test]
        public void minus()
        {
            Type ot = initModule();
            Type st = initNumericModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(double) }, null).Invoke(null, new object[] { (double)10.0 });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "5" });
            var prms = new object[] { "-", o, args, null };
            var ret = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(5.0, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
        }

        [Test]
        public void mul()
        {
            Type ot = initModule();
            Type st = initNumericModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(double) }, null).Invoke(null, new object[] { (double)10.0 });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "5" });
            var prms = new object[] { "*", o, args, null };
            var ret = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(50.0, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
        }

        [Test]
        public void div()
        {
            Type ot = initModule();
            Type st = initNumericModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(double) }, null).Invoke(null, new object[] { (double)10.0 });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "5" });
            var prms = new object[] { "/", o, args, null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(2.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void pow()
        {
            Type ot = initModule();
            Type st = initNumericModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(double) }, null).Invoke(null, new object[] { (double)10.0 });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "3" });
            var prms = new object[] { "**", o, args, null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(1000.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void mod()
        {
            Type ot = initModule();
            Type st = initNumericModule();

            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(double) }, null).Invoke(null, new object[] { (double)10.0 });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "3" });
            var prms = new object[] { "%", o, args, null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(1.0, so.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
        /*
        [Test]
        public void compareToGreater()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)11.0 });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { ">", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void compareToGreaterEqual()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();

            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)11.0 });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { ">=", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void compareToLower()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)11.0 });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { "<", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void compareToLowerEqual()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();

            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)11.0 });

            // 引数なし
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { "<=", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void eq()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });
        
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { "==", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void equal()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            var prms = new object[] { "===", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void between()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            Type at = initArgumentsModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)10.0 });
            var arg1 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)3.0 });
            var arg2 = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)15.0 });

            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg1 });
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg2 });
            var prms = new object[] { "between?", args };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual(true, so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
         */
    }
}
