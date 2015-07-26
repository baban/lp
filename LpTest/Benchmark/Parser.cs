using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace LpTest.Benchmark
{
    /*
    [TestFixture]
    class Parser
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

        [Test]
        public void Block()
        {
            // 初回: 3.8523334
            Type t = initParser();
            var p = t.InvokeMember("Block", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("benchmarkString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do 10 end", 1000 });
        }

        [Test]
        public void Block2()
        {
            // 初回: 3.8523334
            Type t = initParser();
            var p = t.InvokeMember("Block", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("benchmarkString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "do |a,b,c| 10 end", 1000 });
        }

        [Test]
        public void ArgsCall()
        {
            // 1: 15.4567938s
            Type t = initParser();
            var p = t.InvokeMember("ArgsCall", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("benchmarkStrings", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "(10.(==)(10), do ((1.(+)(10))) end, do 10.(+)(10) end)", 10 });
        }

        [Test]
        public void Stmt()
        {
            Type t = initParser();
            var p = t.InvokeMember("Stmt", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("benchmarkString", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "_if(10.(==)(10), do ((1.(+)(10))) end, do 10.(+)(10) end)", 10 });
        }

        [Test]
        public void STMT()
        {
            // 初回: 1.5748601
            // ２回目：1.454678
            Type t = initParser();
            var p = t.InvokeMember("STMT", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField, null, t, null);
            t.GetMethod("benchmarkObject", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { p, "_if(10.(==)(10), do ((1.(+)(10))) end, do 10.(+)(10) end)", 10 });
        }
    }
     * */
}
