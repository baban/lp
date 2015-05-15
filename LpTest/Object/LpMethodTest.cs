using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LpTest.Object
{
    [TestClass]
    class LpMethodTest
    {
        private Type getModule(string name)
        {
            Assembly asm = Assembly.LoadFrom("LP.exe");
            Module mod = asm.GetModule("LP.exe");
            Type t = mod.GetType(name);
            return t;
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initMethodModule()
        {
            return getModule("LP.Object.LpMethod");
        }

        [TestMethod]
        public void initialize()
        {
            Type t = initModule();
            Type mt = initMethodModule();
            // 配列作成
            var o = mt.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string[]), typeof(string[]) }, null).Invoke(null, new object[] { new string[] { }, new string[] { "1", "2", "3" } });
            o = mt.GetMethod("execute", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { o, null });

            Assert.AreEqual(3, o.GetType().InvokeMember("doubleValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
        }
    }
}
