using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace LpTest.Object
{
    [TestFixture]
    class LpKernelTest
    {
        private Type initParser()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.LpParser");
            return t;
        }

        private Type initKernel()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpKernel");
            return t;
        }

        private Type initArgumentsModule()
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType("LP.Object.LpArguments");
            return t;
        }
        /*
        [Test]
        public void print()
        {
            Type t = initParser();
            var p = t.InvokeMember("ARGS", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            var arg = t.GetMethod("parseObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)"\"10\""});

            Type at = initArgumentsModule();
            // 引数なし
            var atypes = new Type[] { };
            var args = at.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, atypes, null).Invoke(null, null);
            args = at.GetMethod("push", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, arg });

            // kernelメソッド呼び出し
            Type k = initKernel();
            var ret = k.GetMethod("print", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { args, args });
            Assert.IsNull(ret);
        }

         */
        [Test]
        public void _if()
        {
            Type t = initParser();
            Type k = initKernel();
            var p = t.InvokeMember("ARGS", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);

            var args = t.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { (string)" true, do 10; end, do 20; end " });
            var ret = k.GetMethod("_if", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { null, args, null });
            //Assert.AreEqual("Numeric", ret.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
            //Assert.AreEqual(10, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
            /*
            args = t.GetMethod("parseArgsObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, (string)" true, 5, 10 " });
            ret = k.GetMethod("_if", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { null, args });
            Assert.AreEqual("Numeric", ret.GetType().InvokeMember("class_name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
            Assert.AreEqual(5, ret.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, ret, null));
             */
        }
    }
}
