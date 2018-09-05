using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;
using Irony.Interpreter;

namespace LpTest.Node
{
    [TestClass]
    public class NumberNodeTest
    {
        [TestMethod]
        public void IntTest()
        {
            var Num = new NumberLiteral("Number");
            Num.AddPrefix("0x", NumberOptions.Hex);
            Num.AddPrefix("0d", NumberOptions.Default);
            Num.AddPrefix("0o", NumberOptions.Octal);
            Num.AddPrefix("0", NumberOptions.Octal);
            Num.AddPrefix("0b", NumberOptions.Binary);

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
