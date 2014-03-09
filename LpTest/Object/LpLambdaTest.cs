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
    [TestFixture]
    class LpLambdaTest
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

        private Type initSymbolModule()
        {
            return getModule("LP.Object.LpSymbol");
        }

        private Type initLambdaModule()
        {
            return getModule("LP.Object.LpLambda");
        }

        [Test]
        public void initialize()
        {
            Type t = initLambdaModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] {  }, null).Invoke(null, new object[] {  });
            Assert.AreEqual( o.GetType().ToString(), "LP.Object.LpObject");
        }

        [Test]
        public void bind()
        {
            Type t = initLambdaModule();
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null).Invoke(null, new object[] { });
            var args = initParser().GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { ":aaa" });
            var prms = new object[] { o, args, null };
            var so = t.GetMethod("bind", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, prms);
            Assert.AreEqual(so.GetType().ToString(), "LP.Object.LpObject");
        }
    }
}
