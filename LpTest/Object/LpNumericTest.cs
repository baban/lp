using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;

namespace LpTest.Object
{
    class LpNumericTest
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

        [Test]
        public void initialize0()
        {
            Type ot = initModule();
            Type t = initNumericModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null).Invoke(null, null);
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual( 0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

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
            var so = st.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual("1", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }

        [Test]
        public void to_s_funcall()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.5 });
            var prms = new object[] { "to_s", null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            Assert.AreEqual("1.5", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
        
        [Test]
        public void display()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
            var so = st.GetMethod("display", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual(null,so);
        }

        [Test]
        public void inspect()
        {
            Type ot = initModule();
            Type st = initNumericModule();
            var types = new Type[] { typeof(double) };
            var o = st.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
            var so = st.GetMethod("inspect", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });
            Assert.AreEqual("1", so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null));
        }
    }
}
