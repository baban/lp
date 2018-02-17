using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LpTest.Ast
{
    [TestClass]
    class LpAstArrayTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        [TestMethod]
        public void toSource()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[1, 2, 3]" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("[1, 2, 3]", s);
        }
    }
}
