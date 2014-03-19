using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using Sprache;

namespace LpTest
{
    [TestFixture]
    class ParserTest
    {
        private Type getModule(string name)
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType(name);
            return t;
        }

        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject");
            return t;
        }

        private Type initArrModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpObject[]");
            return t;
        }

        private Type initKernelModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpKernel");
            return t;
        }

        [Test]
        public void Int()
        {
            Type t = initParser();
            var p = t.InvokeMember("Int", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke( null, new object[]{ p, "10" } );
            Assert.AreEqual( "10", pm );
        }

        [Test]
        public void Decimal()
        {
            Type t = initParser();
            var p = t.InvokeMember("Decimal", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1" });
            Assert.AreEqual("10.1", pm);
        }

        [Test]
        public void DefClass()
        {
            Type t = initParser();
            var p = t.InvokeMember("DefClass", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "class Hoge; 10; end" });
            Assert.AreEqual("Class.new(:Hoge) do 10 end", pm);
        }

        [Test]
        public void Numeric()
        {
            Type t = initParser();
            var p = t.InvokeMember("Numeric", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1" });
            Assert.AreEqual("10.1", pm);

            p = t.InvokeMember("Numeric", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "55" });
            Assert.AreEqual("55", pm);
        }

        [Test]
        public void String()
        {
            Type t = initParser();
            var p = t.InvokeMember("String", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"10\"" });
            Assert.AreEqual("\"10\"", pm);
            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"advg\"" });
            Assert.AreEqual("\"advg\"", pm);
            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"\"" });
            Assert.AreEqual("\"\"", pm);
            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"(lk)\"" });
            Assert.AreEqual("\"(lk)\"", pm);
        }

        [Test]
        public void String2()
        {
            Type t = initParser();
            var p = t.InvokeMember("String", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"aaa\\\"vvv\"" });
            Assert.AreEqual("\"aaa\\\"vvv\"", pm);
        }

        [Test]
        public void String3()
        {
            Type t = initParser();
            var p = t.InvokeMember("String", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"\\\"\\\"\\\"\"" });
            Assert.AreEqual("\"\\\"\\\"\\\"\"", pm);
        }

        [Test]
        public void Symbol()
        {
            Type t = initParser();
            var p = t.InvokeMember("Symbol", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, ":aaa" });
            Assert.AreEqual(":aaa", pm);
        }

        [Test]
        public void Bool()
        {
            Type t = initParser();
            var p = t.InvokeMember("Bool", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "true" });
            Assert.AreEqual("true", pm);

            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "false" });
            Assert.AreEqual("false", pm);
        }

        [Test]
        public void Array0()
        {
            Type t = initParser();
            var p = t.InvokeMember("Array", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[]" });
            Assert.AreEqual("[]", pm);

            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[ ]" });
            Assert.AreEqual("[]", pm);
        }

        [Test]
        public void Array1()
        {
            Type t = initParser();
            var p = t.InvokeMember("Array", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[ 10 ]" });
            Assert.AreEqual("[10]", pm);
        }

        [Test]
        public void ArrayN()
        {
            Type t = initParser();
            var p = t.InvokeMember("Array", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[ 10, 10 ]" });
            Assert.AreEqual("[10,10]", pm);
        }

        [Test]
        public void Hash0()
        {
            Type t = initParser();
            var p = t.InvokeMember("Hash", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "{}" });
            Assert.AreEqual("{}", pm);
        }

        [Test]
        public void Hash1()
        {
            Type t = initParser();
            var p = t.InvokeMember("Hash", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "{ 10 : 10 }" });
            Assert.AreEqual("{10 : 10}", pm);
        }

        [Test]
        public void HashN()
        {
            Type t = initParser();
            var p = t.InvokeMember("Hash", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "{ 10 : 10, 3: 5 }" });
            Assert.AreEqual("{10 : 10,3 : 5}", pm);
        }

        [Test]
        public void Block()
        {
            Type t = initParser();
            var p = t.InvokeMember("Block", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do 10 end" });
            Assert.AreEqual("do 10 end", pm);
        }

        [Test]
        public void Block2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Block", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "() do 10 end" });
            Assert.AreEqual("() do 10 end", pm);

            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(a,b,c) do 10 end" });
            Assert.AreEqual("(a, b, c) do 10 end", pm);
        }

        [Test]
        public void Block3()
        {
            Type t = initParser();
            var p = t.InvokeMember("Block", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do |a,b,c| 10 end" });
            Assert.AreEqual("do |a, b, c| 10 end", pm);
        }


        [Test]
        public void Lambda()
        {
            Type t = initParser();
            var p = t.InvokeMember("Lambda", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "->do 10 end" });
            Assert.AreEqual("->do 10 end", pm);
        }

        [Test]
        public void BOOL()
        {
            Type t = initParser();
            var p = t.InvokeMember("BOOL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "true" });
            var b = o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual(true, b);

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "false" });
            b = o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual(false, b);
        }

        [Test]
        public void InlineComment()
        {
            Type t = initParser();
            var p = t.InvokeMember("InlineComment", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "// hoge\n" });
            Assert.AreEqual( s, "" );
        }

        [Test]
        public void Varcall()
        {
            Type t = initParser();
            var p = t.InvokeMember("Varcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "hoge" });
            Assert.AreEqual(s, "hoge");
        }

        [Test]
        public void BlockComment()
        {
            Type t = initParser();
            var p = t.InvokeMember("BlockComment", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "/**/" });
            Assert.AreEqual(s, "");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "/* hoge */" });
            Assert.AreEqual(s, "");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "/* *aa/aa* ** */" });
            Assert.AreEqual(s, "");
        }

        [Test]
        public void Arg()
        {
            Type t = initParser();
            var p = t.InvokeMember("Arg", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var str = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1" });
            Assert.AreEqual("10.1", str);
        }

        [Test]
        public void Args0()
        {
            Type t = initParser();
            var p = t.InvokeMember("Args", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var ss = t.GetMethod("parseArrString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "" });
            Assert.AreEqual("System.String[]", ss.GetType().ToString());
            var cnt = ss.GetType().InvokeMember("Length", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, ss, null);
            Assert.AreEqual(0, cnt);
        }

        [Test]
        public void Args1()
        {
            Type t = initParser();
            var p = t.InvokeMember("Args", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var ss = t.GetMethod("parseArrString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            Assert.AreEqual("System.String[]", ss.GetType().ToString());
            var cnt = ss.GetType().InvokeMember("Length", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, ss, null);
            Assert.AreEqual(1, cnt);
        }

        [Test]
        public void Args2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Args", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var ss = t.GetMethod("parseArrString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10,10" });
            Assert.AreEqual("System.String[]", ss.GetType().ToString());
            var cnt = ss.GetType().InvokeMember("Length", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, ss, null);
            Assert.AreEqual(2, cnt);
        }

        [Test]
        public void Args3()
        {
            Type t = initParser();
            var p = t.InvokeMember("Args", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var ss = t.GetMethod("parseArrString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10,10,10" });
            Assert.AreEqual("System.String[]", ss.GetType().ToString());
            var cnt = ss.GetType().InvokeMember("Length", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, ss, null);
            Assert.AreEqual(3, cnt);
        }

        [Test]
        public void ArgDecl()
        {
            Type t = initParser();
            var p = t.InvokeMember("ArgDecl", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var ss = t.GetMethod("parseArrString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(a,b,c)" });
            Assert.AreEqual("System.String[]", ss.GetType().ToString());
            var cnt = ss.GetType().InvokeMember("Length", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, ss, null);
            Assert.AreEqual(3, cnt);
        }

        [Test]
        public void ExpVal()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpVal", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10" });
            Assert.AreEqual(s, "10");
        }

        [Test]
        public void ExpVal2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpVal", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"true" });
            Assert.AreEqual(s, "true");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"\"aaaaa\"" });
            Assert.AreEqual(s, "\"aaaaa\"");
        }

        [Test]
        public void ExpVal3()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpVal", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"(1+2)" });
            Assert.AreEqual(s, "(1.(+)(2))");
        }
        /*
        [Test]
        public void ExpClassCall()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpClasscall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"Hoge::Mage" });
            Assert.AreEqual(s, "Hoge::Mage");
        }
        */
        [Test]
        public void ExpMethodcall()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMethodcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.to_s" });
            Assert.AreEqual(s, "10.to_s()");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.to_s()" });
            Assert.AreEqual(s, "10.to_s()");
        }

        [Test]
        public void ExpMethodcall2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMethodcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.to_s(1)" });
            Assert.AreEqual(s, "10.to_s(1)");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.to_s(1,2)" });
            Assert.AreEqual(s, "10.to_s(1, 2)");
        }

        [Test]
        public void ExpMethodcall3()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMethodcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.to_s().to_s()" });
            Assert.AreEqual(s, "10.to_s().to_s()");
        }

        [Test]
        public void ExpMethodcall4()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMethodcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.(+)(10)" });
            Assert.AreEqual(s, "10.(+)(10)");
        }

        [Test]
        public void ExpArrayAt()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpArrayAt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10[1]" });
            Assert.AreEqual(s, "10.([])(1)");
        }

        /*
        [Test]
        public void ExpRange()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpRange", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10...5" });
            Assert.AreEqual(s, "10.(...)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10..5" });
            Assert.AreEqual(s, "10.(..)(5)");
        }
        */

        [Test]
        public void ExpEqual()
        {
            /*
            Type t = initParser();
            var p = t.InvokeMember("ExpEqual", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'a = 5" });
            Assert.AreEqual("('a).(=)(5)", s);
            */
        }

        [Test]
        public void Expr()
        {
            Type t = initParser();
            var p = t.InvokeMember("Expr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10+5" });
            Assert.AreEqual("10.(+)(5)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" 5 * 10 " });
            Assert.AreEqual("5.(*)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" 5 + 10 + 4 " });
            Assert.AreEqual("5.(+)(10).(+)(4)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" 5 + 10 * 4 " });
            Assert.AreEqual("5.(+)(10.(*)(4))", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" 5 * 10 + 4 " });
            Assert.AreEqual("5.(*)(10).(+)(4)", s);
        }

        [Test]
        public void Expr2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Expr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10+5" });
            Assert.AreEqual("10.(+)(5)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" 5 + (10 + 4) " });
            Assert.AreEqual("5.(+)((10.(+)(4)))", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"(5 + 10)" });
            Assert.AreEqual("(5.(+)(10))", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" ( 5 + 10 ) " });
            Assert.AreEqual("(5.(+)(10))", s);

        }

        [Test]
        public void Expr3()
        {
            Type t = initParser();
            var p = t.InvokeMember("Expr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10++" });
            Assert.AreEqual("10.(@++)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10--" });
            Assert.AreEqual("10.(@--)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"!10" });
            Assert.AreEqual("10.(!@)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"+10" });
            Assert.AreEqual("10.(+@)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"~10" });
            Assert.AreEqual("10.(~@)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"not 10" });
            Assert.AreEqual("10.(not@)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 ** 10" });
            Assert.AreEqual("10.(**)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"-10" });
            Assert.AreEqual("10.(-@)()", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 * 10" });
            Assert.AreEqual("10.(*)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 / 10" });
            Assert.AreEqual("10.(/)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 % 10" });
            Assert.AreEqual("10.(%)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 + 10" });
            Assert.AreEqual("10.(+)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 - 10" });
            Assert.AreEqual("10.(-)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 << 10" });
            Assert.AreEqual("10.(<<)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 >> 10" });
            Assert.AreEqual("10.(>>)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 & 10" });
            Assert.AreEqual("10.(&)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 | 10" });
            Assert.AreEqual("10.(|)(10)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 | 10" });
            Assert.AreEqual("10.(|)(10)", s);
        }

        
        [Test]
        public void IfExpr()
        {
            Type t = initParser();
            var p = t.InvokeMember("IfStmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10)end" });
            Assert.AreEqual("_if(10,do  end)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; end" });
            Assert.AreEqual("_if(10,do 10 end)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; 20; end" });
            Assert.AreEqual("_if(10,do 10; 20 end)", s);
        }

        [Test]
        public void IfExprElse()
        {
            Type t = initParser();
            var p = t.InvokeMember("IfStmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; else 10; end" });
            Assert.AreEqual("_if(10,do 10 end,do 10 end)", s);
        }

        [Test]
        public void Function()
        {
            Type t = initParser();
            var p = t.InvokeMember("Function", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def aaa()\nend" });
            Assert.AreEqual("->() do  end.bind(:aaa)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def aaa()\n10\n20\n end" });
            Assert.AreEqual("->() do 10; 20 end.bind(:aaa)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def aaa(a)\n10\n end" });
            Assert.AreEqual("->(a) do 10 end.bind(:aaa)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def aaa(a,b,c)\n10\n end" });
            Assert.AreEqual("->(a, b, c) do 10 end.bind(:aaa)", s);
        }

        [Test]
        public void Funcall()
        {
            Type t = initParser();
            var p = t.InvokeMember("Funcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"print(10)" });
            Assert.AreEqual("print(10)", s);
        }

        [Test]
        public void Funcall2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Funcall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"print(10) do 20; end" });
            Assert.AreEqual("print(10) do 20 end", s);
        }

        [Test]
        public void Fname()
        {
            Type t = initParser();
            var p = t.InvokeMember("Fname", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s2 = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "fuga" });
            Assert.AreEqual(s2, "fuga");
        }

        [Test]
        public void Fname2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Fname", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s1 = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(+)" });
            Assert.AreEqual(s1, "(+)");
        }

        [Test]
        public void Quote()
        {
            Type t = initParser();
            var p = t.InvokeMember("Quote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'10" });
            Assert.AreEqual(s, "'10");
        }

        [Test]
        public void Quote2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Quote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'aaa" });
            Assert.AreEqual(s, "'aaa");
        }

        [Test]
        public void Quote3()
        {
            Type t = initParser();
            var p = t.InvokeMember("Quote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'(1 > 10)" });
            Assert.AreEqual(s, "'(1.(>)(10))");
        }

        [Test]
        public void Quote4()
        {
            Type t = initParser();
            var p = t.InvokeMember("Quote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'(i = 10)" });
            Assert.AreEqual(s, "'(:i.(=)(10))");
        }

        [Test]
        public void QUOTE()
        {
             Type t = initParser();
            var p = t.InvokeMember("QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'(1 > 10)" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("(1.(>)(10))", str);
        }

        [Test]
        public void QuasiQuote()
        {
            Type t = initParser();
            var p = t.InvokeMember("QuasiQuote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`10" });
            Assert.AreEqual("`10", s);
        }

        [Test]
        public void QuasiQuote2()
        {
            Type t = initParser();
            var p = t.InvokeMember("QuasiQuote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`aaa" });
            Assert.AreEqual("`aaa", s);
        }

        [Test]
        public void QuasiQuote3()
        {
            /*
            Type t = initParser();
            var p = t.InvokeMember("QuasiQuote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`?aaa" });
            Assert.AreEqual("`?aaa", s);
            */
        }

        [Test]
        public void QUASI_QUOTE()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`(1 > 10)" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("(1.(>)(10))", str);
        }

        [Test]
        public void QUASI_QUOTE2()
        {
            Type t = initParser();
            var p = t.InvokeMember("EXPR", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)":a.(=)('10)" });

            p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`(1 > ?a)" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("(1.(>)(10))", str);
        }

        [Test]
        public void QUASI_QUOTE3()
        {
            Type t = initParser();
            var p = t.InvokeMember("EXPR", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)":test.(=)('i>10)" });
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)":test.(=)('i>10)" });

            p = t.InvokeMember("QUASI_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`hoge(?test) do  end" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("hoge(i.(>)(10)) do  end", str);
        }

        [Test]
        public void QUESTION_QUOTE()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUESTION_QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"?10" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("10", str);
        }

        [Test]
        public void QuestionQuote()
        {
        /*
            Type t = initParser();
            var p = t.InvokeMember("QuestionQuote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"?(1 > 10)" });
            Assert.AreEqual(s, "?(1.(>)(10))");
        */
        }

        [Test]
        public void Stmt()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10;" });
            Assert.AreEqual(s, "10");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10.0;" });
            Assert.AreEqual(s, "10.0");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"\"hoge\";" });
            Assert.AreEqual(s, "\"hoge\"");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"true;" });
            Assert.AreEqual(s, "true");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'mage;" });
            Assert.AreEqual(s, "'mage");
        }

        [Test]
        public void Stmt2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"[1,2,3];" });
            Assert.AreEqual(s, "[1,2,3]");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"{ 1 : 2, 3 : 4 };" });
            Assert.AreEqual(s, "{1 : 2,3 : 4}");
        }

        [Test]
        public void Stmt3()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"do 10 end" });
            Assert.AreEqual(s, "do 10 end");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"->do 10 end" });
            Assert.AreEqual(s, "->do 10 end");
        }

        [Test]
        public void Stmt4()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def hoge()\nend" });
            Assert.AreEqual(s, "->() do  end.bind(:hoge)");
        }

        [Test]
        public void Stmt5()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var txt = "def hoge()\nif( true )\n 10\n else\n 30\n end\nend";
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)txt });
            Assert.AreEqual(s, "->() do _if(true,do 10 end,do 30 end) end.bind(:hoge)");
        }

        /*
        [Test]
        public void FUNCTION()
        {
            Type t = initParser();

            Type kernel = initKernelModule();
            kernel.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var p = t.InvokeMember("FUNCTION", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "def hoge() 10; end" });

            p = t.InvokeMember("FUNCTION_CALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "hoge()" });
        }
        */
        [Test]
        public void NUMERIC()
        {
            Type t = initParser();
            var p = t.InvokeMember("NUMERIC", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1" });
            var str = o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual( 10.1, str );
        }

        [Test]
        public void STRING()
        {
            Type t = initParser();
            var p = t.InvokeMember("STRING", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"a10f\"" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("a10f", str);
        }

        [Test]
        public void SYMBOL()
        {
            Type t = initParser();
            var p = t.InvokeMember("SYMBOL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, ":aaa" });
            var str = o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("aaa", str);
        }

        [Test]
        public void ARRAY0()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARRAY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[]" });
            var vs = o.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.NotNull(vs);
        }

        [Test]
        public void ARRAY1()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARRAY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10]" });
            var vs = o.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.NotNull(vs);
        }

        [Test]
        public void ARRAY_N()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARRAY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10,5]" });
            var vs = o.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.NotNull(vs);
        }

        [Test]
        public void ARRAY_N2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARRAY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10.(+)(10), 5.(*)(5)]" });
            var vs = o.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.NotNull(vs);
        }

        [Test]
        public void HASH0()
        {
            Type t = initParser();
            var p = t.InvokeMember("HASH", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "{}" });
            var vs = o.GetType().InvokeMember("hashValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.NotNull(vs);
        }

        [Test]
        public void ARGS()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARGS", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10" });
            o = t.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10,20" });
            //o = t.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { "10.(+)(10)" });
        }
        /*
        [Test]
        public void FUNCTION_CALL()
        {
            Type t = initParser();

            Type kernel = initKernelModule();
            kernel.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            var p = t.InvokeMember("FUNCTION_CALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "print(10)" });
        }
        */
        [Test]
        public void FUNCALL()
        {
            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.to_s()" });
            Assert.AreEqual("10", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.5.to_s()" });
            Assert.AreEqual("10.5", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void FUNCALL2()
        {
            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(+)(10)" });
            Assert.AreEqual(20.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "15.(-)(10)" });
            Assert.AreEqual(5.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "15.(*)(10)" });
            Assert.AreEqual(150.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "15.(/)(10)" });
            Assert.AreEqual(1.5, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void FUNCALL3()
        {
            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(**)(3)" });
            Assert.AreEqual(1000.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(%)(3)" });
            Assert.AreEqual(1.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void FUNCALL4()
        {
            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.to_s().to_s()" });
            Assert.AreEqual("10", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(+)(10).to_s()" });
            Assert.AreEqual("20", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void FUNCALL5()
        {
            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(+)(2.(*)(2))" });
            Assert.AreEqual(14.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void FUNCALL6()
        {
            Type t = initParser();
            var p = t.InvokeMember("FUNCALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(10).(+)(10)" });
            Assert.AreEqual(20.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(10.(+)(10)).(+)(5.(*)(5))" });
            Assert.AreEqual(45.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void FUNCTION_CALL()
        {
            getModule("LP.Util.LpIndexer").GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Type t = initParser();
            var p = t.InvokeMember("FUNCTION_CALL", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "print(10)" });
        }

        [Test]
        public void BLOCK()
        {
            Type t = initParser();
            var p = t.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = " do 10; end ";
            var blk = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var stmts = blk.GetType().InvokeMember("statements", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, blk, null);
            List<string> ss = (List<string>)(stmts);
            Assert.AreEqual("10", ss.First());
        }

        [Test]
        public void BLOCK2()
        {
            Type t = initParser();
            var p = t.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = " do 10 + 2; end ";
            var blk = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var stmts = blk.GetType().InvokeMember("statements", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, blk, null);
            List<string> ss = (List<string>)(stmts);
            Assert.AreEqual("10.(+)(2)", ss.First());
        }

        [Test]
        public void BLOCK3()
        {
            Type t = initParser();
            var p = t.InvokeMember("BLOCK", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = "() do 10 + 2; end ";
            var blk = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var stmts = blk.GetType().InvokeMember("statements", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, blk, null);
            List<string> ss = (List<string>)(stmts);
            Assert.AreEqual("10.(+)(2)", ss.First());
        }

        [Test]
        public void LAMBDA()
        {
            Type t = initParser();
            var p = t.InvokeMember("LAMBDA", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var block = "->do 10; end ";
            var blk = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)block });
            var stmts = blk.GetType().InvokeMember("statements", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, blk, null);
            List<string> ss = (List<string>)(stmts);
            Assert.AreEqual("10", ss.First());
        }

        [Test]
        public void STMT()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "true" });
            Assert.AreEqual(true, o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "true" });
            Assert.AreEqual(true, o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "'aaa" });
            Assert.AreEqual("aaa", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10,5]" });
            Assert.NotNull( o.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            // TODO: Hashを実装
        }

        [Test]
        public void STMT2()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do 10; end" });
            Assert.NotNull(o.GetType().InvokeMember("statements", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "->do 10; end" });
            Assert.NotNull(o.GetType().InvokeMember("statements", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            // TODO: Class
            // TODO: Module
        }

        [Test]
        public void STMT3()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            Assert.AreEqual( 10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            // Method呼び出し
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.to_s()" });
            Assert.AreEqual("10", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            // Method呼び出し
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(+)(10)" });
            Assert.AreEqual(20, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(*)(3)" });
            Assert.AreEqual(30, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(+)(3.(*)(3))" });
            Assert.AreEqual(19, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            /*
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10,5].to_s()" });
            Assert.AreEqual("[10, 5]", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"cccc\".to_s()" });
            Assert.AreEqual("cccc", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"dddd\".display()" });
            Assert.Null(o);
            */
        }

        [Test]
        public void STMT4()
        {
            getModule("LP.Util.LpIndexer").GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "print(10)" });
        }

        [Test]
        public void STMT5()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(10).(+)(2.(*)(2))" });
            Assert.AreEqual(14.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(10.(+)(10)).(+)(2.(*)(2))" });
            Assert.AreEqual(24.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void STMT6()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, ":a.(=)(10)" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "a" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void STMT7()
        {
            getModule("LP.Util.LpIndexer").GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var code = "->() do 10; end.bind(:hoge)";
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, code });
            code = "hoge()";
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, code });
        }

        [Test]
        public void VARIABLE_CALL()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(:a).(=)(10)" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            p = t.InvokeMember("VARIABLE_CALL", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "a" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void VARIABLE_CALL2()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(:a).(=)(10)" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "a" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void PROGRAM()
        {
            Type t = initParser();
            var p = t.InvokeMember("PROGRAM", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10; 20" });
            Assert.AreEqual(20.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void PROGRAM2()
        {
            Type t = initParser();
            var p = t.InvokeMember("PROGRAM", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            //var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(:a).(=)(10)" });
            //Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            //o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10; 20" });
            //Assert.AreEqual(20.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void PROGRAM3()
        {
            Type t = initParser();
            var p = t.InvokeMember("PROGRAM", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"->() do\nend.bind(:aaa)" });
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def hoge()\nend" });
        }

        [Test]
        public void PROGRAM4()
        {
            getModule("LP.Util.LpIndexer").GetMethod("initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { });
            Type t = initParser();
            var p = t.InvokeMember("PROGRAM", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def hoge(a)\nend;hoge(10)" });

            //o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"def hoge2(a)\n print(a)\n end;hoge2(10)" });
        }
        
        [Test]
        public void Parse_Error()
        {
            Type t = initParser();
            var p = t.InvokeMember("SYMBOL", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            Assert.Catch(delegate() {
                t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            });
        }

        [Test]
        public void Parse_Error2()
        {
            Type t = initParser();
            var p = t.InvokeMember("Program", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            Assert.Catch(delegate()
            {
                t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "1.....0" });
            });
        }

    }
}
