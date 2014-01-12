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
        private Type initParser() {
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
            var pm = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, ":aaaa" });
            Assert.AreEqual(":aaaa", pm);
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

        [Test]
        public void ExpSquare()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpSquare", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10**5" });
            Assert.AreEqual(s, "10.(**)(5)");
        }

        [Test]
        public void ExpAdditive()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAdditive", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10+5" });
            Assert.AreEqual(s, "10.(+)(5)");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10-5" });
            Assert.AreEqual(s, "10.(-)(5)");
        }

        [Test]
        public void ExpAdditive2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAdditive", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10*5" });
            Assert.AreEqual(s, "10.(*)(5)");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 / 5" });
            Assert.AreEqual(s, "10.(/)(5)");
        }

        [Test]
        public void ExpAdditive3()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAdditive", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 + 5 * 3" });
            Assert.AreEqual(s, "10.(+)(5.(*)(3))");
        }

        [Test]
        public void ExpAdditive4()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAdditive", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 + 5 + 3" });
            Assert.AreEqual(s, "10.(+)(5).(+)(3)");
        }

        [Test]
        public void ExpAdditive5()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAdditive", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 ** 3" });
            Assert.AreEqual(s, "10.(**)(3)");
        }

        [Test]
        public void ExpMul()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMul", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10*5" });
            Assert.AreEqual(s, "10.(*)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 / 5" });
            Assert.AreEqual(s, "10.(/)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 % 5" });
            Assert.AreEqual(s, "10.(%)(5)");
        }

        [Test]
        public void ExpMul2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpMul", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10**5" });
            Assert.AreEqual(s, "10.(**)(5)");
        }

        [Test]
        public void ExpShift()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpShift", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10>>5" });
            Assert.AreEqual(s, "10.(>>)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 << 5" });
            Assert.AreEqual(s, "10.(<<)(5)");
        }

        [Test]
        public void ExpAnd()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAnd", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10&5" });
            Assert.AreEqual(s, "10.(&)(5)");
        }

        [Test]
        public void ExpInclusiveOr()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpInclusiveOr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10|5" });
            Assert.AreEqual(s, "10.(|)(5)");
        }

        [Test]
        public void ExpExclusiveOr()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpExclusiveOr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 < 5" });
            Assert.AreEqual("10.(<)(5)", s);
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 <= 5" });
            Assert.AreEqual("10.(<=)(5)", s);
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 > 5" });
            Assert.AreEqual("10.(>)(5)", s);
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 >= 5" });
            Assert.AreEqual("10.(>=)(5)", s);
        }

        [Test]
        public void ExpEquality()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpEquality", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 <=> 5" });
            Assert.AreEqual(s, "10.(<=>)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 === 5" });
            Assert.AreEqual(s, "10.(===)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 == 5" });
            Assert.AreEqual(s, "10.(==)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 != 5" });
            Assert.AreEqual(s, "10.(!=)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 =~ 5" });
            Assert.AreEqual(s, "10.(=~)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 !~ 5" });
            Assert.AreEqual(s, "10.(!~)(5)");

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 == 5 == 6" });
            Assert.AreEqual(s, "10.(==)(5).(==)(6)");
        }

        [Test]
        public void ExpLogicalAnd()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpLogicalAnd", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 && 5" });
            Assert.AreEqual(s, "10.(&&)(5)");
        }

        [Test]
        public void ExpLogicalAnd2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpLogicalAnd", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 & 5" });
            Assert.AreEqual(s, "10.(&)(5)");
        }

        [Test]
        public void ExpLogicalOr()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpLogicalOr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 || 5" });
            Assert.AreEqual(s, "10.(||)(5)");
        }

        [Test]
        public void ExpLogicalOr2()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpLogicalOr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 | 5" });
            Assert.AreEqual(s, "10.(|)(5)");
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
        public void ExpAndOr()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpAndOr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 and 5" });
            Assert.AreEqual(s, "10.(and)(5)");
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10 or 5" });
            Assert.AreEqual(s, "10.(or)(5)");
        }
        /*
        [Test]
        public void ExpEqual()
        {
            Type t = initParser();
            var p = t.InvokeMember("ExpEqual", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"a = 5" });
            Assert.AreEqual("a.(=)(5)", s);
        }
        */

        [Test]
        public void Expr()
        {
            Type t = initParser();
            var p = t.InvokeMember("Expr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"10+5" });
            Assert.AreEqual("10.(+)(5)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" 5 * 10 " });
            Assert.AreEqual("5.(*)(10)", s);
        }

        [Test]
        public void IfExpr()
        {
            Type t = initParser();
            var p = t.InvokeMember("IfExpr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10)end" });
            Assert.AreEqual("if(10,do  end)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; end" });
            Assert.AreEqual("if(10,do 10 end)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; 20; end" });
            Assert.AreEqual("if(10,do 10; 20 end)", s);
        }

        [Test]
        public void IfExprElse()
        {
            Type t = initParser();
            var p = t.InvokeMember("IfExpr", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; else 10; end" });
            Assert.AreEqual("if(10,do 10 end,do 10 end)", s);
            /*
            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; end" });
            Assert.AreEqual("if(10,do 10 end)", s);

            s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"if(10) 10; 20; end" });
            Assert.AreEqual("if(10,do 10;20 end)", s);
            */
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
            var o = t.GetMethod("parseArrObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            var ar = o.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null);
            Assert.AreEqual("arguments", ar);
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
            Assert.AreEqual(s1, "+");
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
        public void STMT()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            Assert.AreEqual(10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "\"aaa\"" });
            Assert.AreEqual("aaa", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, ":aaa" });
            Assert.AreEqual("aaa", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10,5]" });
            Assert.NotNull( o.GetType().InvokeMember("arrayValues", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        [Test]
        public void STMT2()
        {
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10" });
            Assert.AreEqual( 10.0, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.to_s()" });
            Assert.AreEqual("10", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "10.(+)(10)" });
            Assert.AreEqual(20, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));

            //o = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "[10,5].to_s()" });
            //Assert.AreEqual("[10,5]", o.GetType().InvokeMember("stringValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }

        /*
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

        [Test]
        public void Quote()
        {
            Type t = initParser();
            var p = t.InvokeMember("Quote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"'10" });
            Assert.AreEqual(s, "10");
        }

        [Test]
        public void QuasiQuote()
        {
            Type t = initParser();
            var p = t.InvokeMember("QuasiQuote", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var s = t.GetMethod("parseString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"`10" });
            Assert.AreEqual( "10", s );
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

        /*
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
            public void ExpValue()
            {
                TestRubyParser.Object.LpIndexer.init();
                TestRubyParser.LpParser.ExpValue.Parse(" a = 19 ");
                TestRubyParser.Object.LpObject o = TestRubyParser.Object.LpIndexer.get("a");
                Assert.AreEqual( o.doubleValue, 19.0 );
            }
            */
    }
}
