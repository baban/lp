using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using Sprache;

namespace LpTest.Ast
{
    [TestFixture]
    class LpAstFuncallTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        [Test]
        public void toSource1()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "hoge(1,2)" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, null);
            Assert.AreEqual("hoge(1, 2)", s);
        }
        /*
        [Test]
        public void toSource2()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "mage(1) do |i| 10 end" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, null);
            Assert.AreEqual("mage(1) do |i| 10 end", s);
        }
        */
    }
}
