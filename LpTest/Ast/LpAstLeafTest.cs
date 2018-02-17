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
    class LpAstLeafTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        [TestMethod]
        public void toSourcetoBool()
        {
            Type t = initParser();
            var p = t.InvokeMember("BOOL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "true" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[]{ false });
            Assert.AreEqual("true",s);
        }
        /*
        [Test]
        public void toSourcetoNumeric()
        {
            Type t = initParser();
            var p = t.InvokeMember("NUMERIC", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("10", s);
        }

        [Test]
        public void toSourcetoQuote()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "'10" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("'10", s);
        }

        [Test]
        public void toSourcetoQuote2()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "'(10)" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("'10", s);
        }

        [Test]
        public void toSourcetoQuote3()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "'print(10)" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("'print(10)", s);
        }

        [Test]
        public void toSourcetoPrimary()
        {
            Type t = initParser();
            var p = t.InvokeMember("PRIMARY", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("10", s);
        }

        [Test]
        public void toSourcetoQuasiQuote()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "`10" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("`10", s);
        }

        [Test]
        public void toSourcetoQuasiQuote2()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "`(10)" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { true });
            Assert.AreEqual("`10", s);
        }

        [Test]
        public void toSourcetoQuasiQuote3()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "`(print(10))" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("`print(10)", s);
        }

        [Test]
        public void toSourcetoQuasiQuote4()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "`(10.(>)(5))" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("`10.(>)(5)", s);
        }

        [Test]
        public void toSourcetoQuasiQuote5()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "`(10.(+)(?5))" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("`10.(+)(?5)", s);
        }

        [Test]
        public void toSourcetoQuasiQuote6()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "`(10.(+)(?(10.(+)(2))))" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("`10.(+)(?(10.(+)(2)))", s);
        }

        [Test]
        public void toSourcetoQuestionQuote()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUESTION_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "?a" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            Assert.AreEqual("?a", s);
        }

        [Test]
        public void toSourcetoQuestionQuote2()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUESTION_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "?10" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { true });
            Assert.AreEqual("10", s);
        }

        [Test]
        public void toSourcetoQuestionQuote3()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUESTION_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "?(1.(+)(2))" });
            var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { true });
            Assert.AreEqual("3", s);
        }
        */
    }
}
