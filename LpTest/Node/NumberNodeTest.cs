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
            Type t = mod.GetType("LP.LpParser.LpGrammer");
            return t;
        }

        [TestMethod]
        public void IntTest()
        {
            Type t = initParser();
            
            var node = t.GetMethod("createNumberLiteral", BindingFlags.NonPublic | BindingFlags.Static);
            //var node = t.GetMethod("createNumberLiteral", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { });

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
