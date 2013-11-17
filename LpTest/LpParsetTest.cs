﻿using System;
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
        public Type initParser() {
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

        [Test]
        public void Int()
        {
            Type t = initParser();
            var p = t.InvokeMember("Int", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke( null, new object[]{ p, "10" } );
            Assert.AreEqual( "10", pm );
            pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, " 10 " });
            Assert.AreEqual("10", pm);
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
        public void NUMERIC()
        {
            Type t = initParser();
            var p = t.InvokeMember("NUMERIC", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1" });
            var str = o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual( 10.1, str );
        }

        [Test]
        public void NUMERIC2()
        {
            Type t = initParser();
            var p = t.InvokeMember("NUMERIC", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1 " });
            var str = o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual(10.1, str);

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, " 10 " });
            str = o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual(10.0, str);

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, " 12.5 " });
            str = o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual(12.5, str);
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
        public void ARGdouble()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARG", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.1" });
            var str = o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual(10.1, str);
        }

        [Test]
        public void SepArg()
        {
            Type t = initParser();
            var p = t.InvokeMember("SepArg", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var str = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, ",10" });
            Assert.AreEqual("10", str);
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
        public void ARGS()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARGS", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseArrObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            var ar = o.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("arguments", ar);
        }
        
        [Test]
        public void Fname()
        {
            Type t = initParser();
            var p = t.InvokeMember("Fname", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s1 = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(+)" });
            Assert.AreEqual(s1, "(+)");
            var s2 = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "fuga" });
            Assert.AreEqual(s2, "fuga");
        }

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
        public void ExpAdditive()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAdditive", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10+5" });
            Assert.AreEqual(s, "10.(+)(5)");
        }

        [Test]
        public void ExpMul()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMul", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10*5" });
            Assert.AreEqual(s, "10.(*)(5)");
        }

        [Test]
        public void Stmt()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10;" });
            Assert.AreEqual(s, "10");
        }

        [Test]
        public void Expr()
        {
            Type t = initParser();
            var p = t.InvokeMember("Expr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10+5" });
            Assert.AreEqual(s, "10.(+)(5)");
        }
        /*
        
        [Test]
        public void Funcall3()
        {
            TestRubyParser.Object.LpObject o = TestRubyParser.LpParser.Funcall.Parse("10.(+)(15)");
            Assert.AreEqual( o.doubleValue, 25.0 );
        }

        [Test]
        public void Block()
        {
            TestRubyParser.Object.LpObject o;
            o = TestRubyParser.LpParser.Block.Parse("do end");
            Assert.AreEqual(o.stmts.Count, 0);
            o = o.funcall("execute", null);
            o = TestRubyParser.LpParser.Block.Parse("do 10; end");
            Assert.AreEqual(o.stmts.Count, 1);
            Assert.AreEqual(o.stmts[0], "10");
            o = o.funcall("execute", null);
            Assert.AreEqual(o.doubleValue, 10.0);
            o = TestRubyParser.LpParser.Block.Parse("do 10; 5; end");
            Assert.AreEqual(o.stmts.Count, 2);
            o = o.funcall("execute", null);
            Assert.AreEqual(o.doubleValue, 5.0);
        }
        */
            /*
            [Test]
            public void Quote()
            {
                string token = TestRubyParser.LpParser.Quote.Parse("'10");
                Assert.AreEqual(token, "'10");
            }
            [Test]
            public void QuasiQuote()
            {
                TestRubyParser.Object.LpObject o;
                o = TestRubyParser.LpParser.Quasiquote.Parse("`10");
                Assert.AreEqual( o.doubleValue, 10.0 );
                o = TestRubyParser.LpParser.Quasiquote.Parse("`5 + ?name");
                Assert.AreEqual(o.doubleValue, 11.0);
                //o = TestRubyParser.LpParser.Quasiquote.Parse("`a+b");
                //Assert.AreEqual(o, "`a+b");
                //o = TestRubyParser.LpParser.Quasiquote.Parse("`a+?b");
                //Assert.AreEqual(o, "`a+10");
                //TestRubyParser.Object.LpObject o = new TestRubyParser.Object.LpNumeric().initialize(10);
                //TestRubyParser.Object.LpIndexer.set("name", o );
                //o = TestRubyParser.LpParser.ExpAdditive.Parse("19 + 2");
                //Assert.AreEqual(o,"19+2");
            }
            [Test]
            public void Identifier()
            {
                string token = TestRubyParser.LpParser.Identifier.Parse("abcd");
                Assert.AreEqual( token, "abcd" );
            }

            [Test]
            public void Varname()
            {
                string token = TestRubyParser.LpParser.Varname.Parse("abcd");
                Assert.AreEqual(token, "abcd");
            }

            [Test]
            public void Numeric()
            {
                TestRubyParser.Object.LpObject o = TestRubyParser.LpParser.Numeric.Parse("19");
                Assert.AreEqual(o.doubleValue, 19.0);
            }

            [Test]
            public void NumericS()
            {
                string s = TestRubyParser.LpParser.NumericS.Parse("19");
                Assert.AreEqual(s, "19.0");
            }
            */
            /*
            [Test]
            public void ExpAdditive()
            {
                TestRubyParser.Object.LpObject o;
                o = TestRubyParser.LpParser.ExpAdditive.Parse("19");
                Assert.AreEqual(o.doubleValue, 19.0);
                o = TestRubyParser.LpParser.ExpAdditive.Parse("19 + 2");
                Assert.AreEqual(o.doubleValue, 21.0);
            }
            [Test]
            public void ExpMult()
            {
                TestRubyParser.Object.LpObject o;
                o = TestRubyParser.LpParser.ExpAdditive.Parse("19");
                Assert.AreEqual(o.doubleValue, 19.0);
                o = TestRubyParser.LpParser.ExpAdditive.Parse("19 + 2");
                Assert.AreEqual(o.doubleValue, 21.0);
                o = TestRubyParser.LpParser.ExpAdditive.Parse("19 * 2");
                Assert.AreEqual(o.doubleValue, 38.0);
                o = TestRubyParser.LpParser.ExpAdditive.Parse("4 + 19 * 2");
                Assert.AreEqual(o.doubleValue, 42.0);
            }

            [Test]
            public void ExpValue()
            {
                TestRubyParser.Object.LpIndexer.init();
                TestRubyParser.LpParser.ExpValue.Parse(" a = 19 ");
                TestRubyParser.Object.LpObject o = TestRubyParser.Object.LpIndexer.get("a");
                Assert.AreEqual( o.doubleValue, 19.0 );
            }
             * */
    }
}
