using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;

namespace LpTest
{
    [TestFixture]
    public class TestTest
    {
        [Test]
        public void test()
        {
            Assert.AreEqual(true, true);
        }
        /*
        [Test]
        public void private_test()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            System.Type type = mod.GetType("LP.Object.LpObject");
            Object o = Activator.CreateInstance(type);
            Console.WriteLine(o.GetType().InvokeMember("fuga", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            Assert.AreEqual(o.GetType().InvokeMember("fuga", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, o, null), true );
            int result = (int)o.GetType().InvokeMember("SumWithState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, new object[] { 10 });
            Assert.AreEqual( result, 1 );
        }
        [Test]
        public void private_static_test()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            Assert.AreEqual(t.GetMethod("hoge", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null), 1);
        }
         */
    }
}
