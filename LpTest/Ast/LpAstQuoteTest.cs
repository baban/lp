﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Reflection;
using Sprache;

namespace LpTest.Ast
{
    [TestFixture]
    class LpAstQuoteTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        [Test]
        public void toSource()
        {
            Type t = initParser();
            var p = t.InvokeMember("QUOTE", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var node = t.GetMethod("parseToNode", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "'10" });
            //var s = node.GetType().GetMethod("toSource", BindingFlags.Public | BindingFlags.Instance).Invoke(node, new object[] { false });
            //Assert.AreEqual("true", s);
        }
    }
}