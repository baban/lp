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
        private Type initModule(){
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
            var prms = new object[] { o, o };
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
            var prms = new object[] { "to_s", null };
            var so = o.GetType().InvokeMember("funcall", BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, prms);
            var str = so.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, so, null);
            var r = new Regex(@"<obj \w+?>");
            Assert.True(r.IsMatch(str.ToString()));
        }
    }
}
