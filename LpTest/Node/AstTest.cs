using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Irony.Parsing;
using Irony.Interpreter;

namespace LpTest.Node
{
    [TestClass]
    public class AstTest
    {
        //string code = "/* 111 */ 2";
        //string code = "nl";
        //string code = "/regex/";
        //string code = "a=1; a";
        //string code = "let a=1; a";
        //string code = "b? = 2; b?";
        //string code = "@a = 3; @a";
        //string code = "@@a = 4; @@a";
        //string code = "1; 2; 3";
        //string code = "a='(1+2); ?a";
        //string code = "a='(1+2); `(1+3)";
        //string code = "a='(2+3); `(1+?a)";
        //string code = "Console";
        //string code = "Console.WriteLine(\"Hello,World\")";
        //string code = "abc=1+5*5; abc";
        //string code = "def bbb(a,b,c) 1; 2; c end; bbb(1,2,3)";
        //string code = "def hoge(a, b) 1; 2; 3 end"

        ScriptApp staticApp = null;
        Parser staticParser = null;

        private Parser initParser()
        {
            if (staticParser != null) return staticParser;

            var parser = new LP.Parser.LpGrammer();
            var language = new LanguageData(parser);
            ScriptApp app = new ScriptApp(language);
            staticApp = app;
            staticParser = app.Parser;
            return staticParser;
        }

        private ParseTree parse(string code) {
            var parser = initParser();
            var tree = parser.Parse(code);
            return tree;
        }

        private LP.Object.LpObject evaluate(string code)
        {
            var parser = initParser();
            var tree = parser.Parse(code);
            var ret = staticApp.Evaluate(tree) as LP.Object.LpObject;
            return ret;
        }

        [TestMethod]
        public void parseNumberTest()
        {
            var code = "1";
            var tree = parse(code);
            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void parseSymbolTest()
        {
            Assert.IsNotNull(parse(":aaa"));
        }

        [TestMethod]
        public void parseStringTest()
        {
            var code = "\"Hello\"";
            var tree = parse(code);
            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void parseBooleanTest()
        {
            var code = "true";
            var tree = parse(code);
            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void parseArrayTest()
        {
            var code = "[]";
            var tree = parse(code);
            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void parseHashTest()
        {
            var code = "{}";
            var tree = parse(code);
            Assert.IsNotNull(tree);
        }

        [TestMethod]
        public void parseBlockTest()
        {
            Assert.IsNotNull(parse("do |a| end"));
            Assert.IsNotNull(parse("do |a| 1 end"));
            Assert.IsNotNull(parse("do |a| 1; 2 end"));
            Assert.IsNotNull(parse("do |a| 1\n end"));
            Assert.IsNotNull(parse("do |a| 1\n 2 end"));
            Assert.IsNotNull(parse("do |a|; end"));
            Assert.IsNotNull(parse("do |a|\n end"));
            // Assert.IsNotNull(parse("do end"));
        }

        [TestMethod]
        public void parseLambdaTest()
        {
            Assert.IsNotNull(parse("-> do |a| end"));
        }

        [TestMethod]
        public void parseVariableSetTest()
        {
            Assert.IsNotNull(parse("a=1"));
            Assert.IsNotNull(parse("a=1;"));
            Assert.IsNotNull(parse("let a=1;"));
            Assert.IsNotNull(parse("b? = 2"));
            Assert.IsNotNull(parse("@a = 3"));
            Assert.IsNotNull(parse("@@a = 3"));
            Assert.IsNotNull(parse("let a;"));
        }

        [TestMethod]
        public void exprTest()
        {
            Assert.IsNotNull(parse("1+a"));
            Assert.IsNotNull(parse("1+1"));
            Assert.IsNotNull(parse("2*3"));
            Assert.IsNotNull(parse("!true"));
            Assert.IsNotNull(parse("1+2*3+4"));
        }

        [TestMethod]
        public void methodCallTest()
        {
            Assert.IsNotNull(parse("1.to_s()"));
        }

        [TestMethod]
        public void funcallTest()
        {
            Assert.IsNotNull(parse("print(5)"));
        }

        [TestMethod]
        public void ifDefuneFunctionTest()
        {
            Assert.IsNotNull(parse("def hoge() end"));
            Assert.IsNotNull(parse("def hoge(a) 1; 2; 3 end"));
            Assert.IsNotNull(parse("def hoge(a?) end"));
            Assert.IsNotNull(parse("def hoge(a!) end"));
            Assert.IsNotNull(parse("def hoge(a,b) end"));
            Assert.IsNotNull(parse("def hoge(*a) end"));
            Assert.IsNotNull(parse("def hoge(&a) end"));
            Assert.IsNotNull(parse("def hoge(a, *b) end"));
            Assert.IsNotNull(parse("public def hoge(a) 1; 2; 3 end"));
        }


        [TestMethod]
        public void ifClassModuleTest()
        {
            Assert.IsNotNull(parse("class Aaa end"));
            Assert.IsNotNull(parse("class Aaa; end"));
            Assert.IsNotNull(parse("class Aaa +B; end"));
            Assert.IsNotNull(parse("class Aaa 1;2;3 end"));
            Assert.IsNotNull(parse("class A < B; 1;2;3 end"));
            Assert.IsNotNull(parse("public class A; 1;2;3 end"));
            Assert.IsNotNull(parse("module Aaa; 1;2;3 end"));
            Assert.IsNotNull(parse("public module Aaa; 1;2;3 end"));
        }

        [TestMethod]
        public void ifStmtTest()
        {
            Assert.IsNotNull(parse("if true; 1 end"));
            Assert.IsNotNull(parse("if false; 1 else 2 end"));
            Assert.IsNotNull(parse("if false; 1 elsif true; 2 end"));
        }

        [TestMethod]
        public void caseStmtTest()
        {
            Assert.IsNotNull(parse("case 1; end"));
            Assert.IsNotNull(parse("case false; else 1 end"));
            Assert.IsNotNull(parse("case 1; when 1; 3 end"));
        }
    }
}
