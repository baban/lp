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
    class LpAstBlockTest
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
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do end" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, null);
            Assert.AreEqual("do  end", s);
        }

        [Test]
        public void toSource2()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do 10; 15 end" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, null);
            Assert.AreEqual("do 10; 15 end", s);
        }

        [Test]
        public void toSource3()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do |a,b,c| 10 end" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, null);
            Assert.AreEqual("do |a,b,c| 10 end", s);
        }
    }
}
