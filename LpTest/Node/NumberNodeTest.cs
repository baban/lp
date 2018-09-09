using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;
using Irony.Interpreter;
using System.Reflection;

namespace LpTest.Node
{
    [TestClass]
    public class NumberNodeTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Parser.LpGrammer");
            return t;
        }

        [TestMethod]
        public void IntTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createNumberLiteral", BindingFlags.NonPublic | BindingFlags.Static);
            NumberLiteral Num = (NumberLiteral)m.Invoke(null, null);
           
            Parser parser; Token token;
            parser = TestHelper.CreateParser(Num);

            token = parser.ParseInput("10");
            Assert.AreEqual(token.Value, 10);

            token = parser.ParseInput("0d10");
            Assert.AreEqual(token.Value, 10);

            token = parser.ParseInput("0x0a");
            Assert.AreEqual(token.Value, 10);

            token = parser.ParseInput("0b1010");
            Assert.AreEqual(token.Value, 10);
        }
    }
}
