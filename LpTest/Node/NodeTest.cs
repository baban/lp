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
    public class NodeTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Parser.LpGrammer");
            return t;
        }

        [TestMethod]
        public void numberLiteralTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createNumberLiteral", BindingFlags.NonPublic | BindingFlags.Static);
            NumberLiteral Num = (NumberLiteral)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(Num);

            Token token;

            token = parser.ParseInput("10");
            Assert.AreEqual(token.Value, 10);

            token = parser.ParseInput("0d10");
            Assert.AreEqual(token.Value, 10);

            token = parser.ParseInput("0x0a");
            Assert.AreEqual(token.Value, 10);

            token = parser.ParseInput("0b1010");
            Assert.AreEqual(token.Value, 10);
        }

        [TestMethod]
        public void stringLiteralTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createStringLiteral", BindingFlags.NonPublic | BindingFlags.Static);
            StringLiteral literal = (StringLiteral)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(literal);
            Token token;

            token = parser.ParseInput("\"hoge\"");
            Assert.AreEqual(token.Value, "hoge");

            token = parser.ParseInput("'hoge'");
            Assert.AreEqual(token.Value, "hoge");

            token = parser.ParseInput("%[hoge]");
            Assert.AreEqual(token.Value, "hoge");
        }
        
        [TestMethod]
        public void createIdentifierTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createIdentifier", BindingFlags.NonPublic | BindingFlags.Static);
            var expr = (IdentifierTerminal)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(expr);
            Token token;

            token = parser.ParseInput("hoge");
            Assert.AreEqual(token.Text, "hoge");

            token = parser.ParseInput("Hoge");
            Assert.AreNotEqual(token.Text, "Hoge");

            token = parser.ParseInput("var_name");
            Assert.AreEqual(token.Text, "var_name");

            token = parser.ParseInput("VAR_NAME");
            Assert.AreNotEqual(token.Text, "VAR_NAME");
        }

        [TestMethod]
        public void createExclamationIdentifierTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createExclamationIdentifier", BindingFlags.NonPublic | BindingFlags.Static);
            var expr = (IdentifierTerminal)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(expr);
            Token token;

            token = parser.ParseInput("hoge!");
            Assert.AreEqual(token.Text, "hoge!");

            token = parser.ParseInput("Hoge!");
            Assert.AreNotEqual(token.Text, "Hoge!");
        }

        [TestMethod]
        public void createQuestionIdentifierTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createQuestionIdentifier", BindingFlags.NonPublic | BindingFlags.Static);
            var expr = (IdentifierTerminal)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(expr);
            Token token;

            token = parser.ParseInput("hoge?");
            Assert.AreEqual(token.Text, "hoge?");

            token = parser.ParseInput("Hoge?");
            Assert.AreNotEqual(token.Text, "Hoge?");
        }

        [TestMethod]
        public void varNameTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createVarName", BindingFlags.NonPublic | BindingFlags.Static);
            var expr = (BnfExpression)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(expr);
            Token token;

            token = parser.ParseInput("foo");
            Assert.AreEqual(token.Text, "foo");

            token = parser.ParseInput("bar!");
            Assert.AreEqual(token.Text, "bar!");

            token = parser.ParseInput("baz?");
            Assert.AreEqual(token.Text, "baz?");

        }

        [TestMethod]
        public void createConstIdentifierTest()
        {
            Type t = initParser();
            var m = t.GetMethod("createConstIdentifier", BindingFlags.NonPublic | BindingFlags.Static);
            var expr = (IdentifierTerminal)m.Invoke(null, null);

            Parser parser = TestHelper.CreateParser(expr);
            Token token;

            token = parser.ParseInput("Hoge");
            Assert.AreEqual(token.Text, "Hoge");

            token = parser.ParseInput("hoge");
            Assert.AreNotEqual(token.Text, "hoge");

            token = parser.ParseInput("CONST_VAR_NAME");
            Assert.AreEqual(token.Text, "CONST_VAR_NAME");
        }
    }
}
