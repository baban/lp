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
    class LpObjectTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }
        
        [Test]
        public void initialize()
        {
            Type t = initModule();
            Assert.AreEqual(t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null).GetType().ToString(), "LP.Object.LpObject");
        }

        [Test]
        public void to_s()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual( str, null );
            
            // 引数なし
            var prms = new object[] { o, null, null };
            var so = t.GetMethod("to_s", BindingFlags.Static | BindingFlags.NonPublic ).Invoke( null, prms );
            str = so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.True( r.IsMatch( str.ToString() ) );
        }

        [Test]
        public void funcall()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            // 引数なし
            var prms = new object[] { "to_s", null, null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            var str = so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.True(r.IsMatch(str.ToString()));
        }

        [Test]
        public void funcall_no_method_raise()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            // 引数なし
            var prms = new object[] { "to_s_error", null };
            Assert.Catch(delegate() { o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms); });
        }

        [Test]
        public void is_nil()
        {
            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            // 引数なし
            var prms = new object[] { o, null, null };
            var so = t.GetMethod("is_nil", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, prms);
            var b = so.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            Assert.False( (bool)b );
        }

        [Test]
        public void define_method()
        {
            var pm = initParser();
            var p = pm.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, pm, null);

            Type t = initModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var args  = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "\"hoge\"" });
            var block = pm.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" do 10; end " });

            // 引数なし
            var prms = new object[] { o, args, block };
            var so = t.GetMethod("define_method", BindingFlags.Static | BindingFlags.NonPublic).Invoke(o, prms);
        }

        [Test]
        public void send()
        {
            // TODO: 未実装
        }

        [Test]
        public void alias()
        {
            // TODO: 未実装
        }

        [Test]
        public void methods()
        {
            // TODO: 未実装
        }

        [Test]
        public void is_a()
        {
            // TODO: 未実装
        }
    }
}
