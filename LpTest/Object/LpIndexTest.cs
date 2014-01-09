using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    class LpIndexTest
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

        private Type initIndexerModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpIndexer");
            return t;
        }

        [Test]
        public void simpleSetGet()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type t = initIndexerModule();

            var types = new Type[] { typeof(double) };
            var v = nt.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { (double)1.0 });
            
            var indexer = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
            var prms = new object[]{ (string)"hoge", v };
            t.GetMethod("set", BindingFlags.Static | BindingFlags.Public).Invoke(null, prms);
            var prms2 = new string[] { (string)"hoge" };
            var o = t.GetMethod("get", BindingFlags.Static | BindingFlags.Public).Invoke(null, prms2);
 
            Assert.AreEqual(1.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
}
