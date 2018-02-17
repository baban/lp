using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LpTest.Object
{
    class LpParser
    {
        private Type getModule(string name)
        {
            return Assembly.LoadFrom("LP.exe").GetModule("LP.exe").GetType(name);
        }

        private Type initParser()
        {
            return getModule("LP.LpParser");
        }

        private Type initModule()
        {
            return getModule("LP.Object.LpObject");
        }

        private Type initBoolModule()
        {
            return getModule("LP.Object.LpBool");
        }

        [TestMethod]
        public void initialize1()
        {
            
            var pm = initParser();
            var p = pm.InvokeMember("CaseStmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, pm, null);
            
            /*
            var types = new Type[] { typeof(bool) };
            var o = t.GetMethod("initialize", BindingFlags.Static | BindingFlags.Public, null, types, null).Invoke(null, new object[] { true });
            Assert.AreEqual("LP.Object.LpObject", o.GetType().ToString());
            Assert.AreEqual(true, o.GetType().InvokeMember("boolValue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
             */
        }
    }
}
