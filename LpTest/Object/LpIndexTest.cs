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
    class LpIndexTest
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

        private Type initNumericModule()
        {
            return getModule("LP.Object.LpNumeric");
        }

        private Type initIndexerModule()
        {
            return getModule("LP.Object.LpIndexer");
        }

        [TestMethod]
        public void simpleSetGet()
        {
            Type ot = initModule();
            Type nt = initNumericModule();
            Type t = initIndexerModule();

            var types = new Type[] { typeof(double) };
            var v = new PrivateType(nt).InvokeStatic("initialize", new object[] { (double)1.0 });

            var indexer = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
            var prms = new object[] { (string)"hoge", v };
            t.GetMethod("set", BindingFlags.Static | BindingFlags.Public).Invoke(null, prms);
            var prms2 = new string[] { (string)"hoge" };
            var o = t.GetMethod("get", BindingFlags.Static | BindingFlags.Public).Invoke(null, prms2);

            Assert.AreEqual(1.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
}
