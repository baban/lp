using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lp;
using System.Reflection;

namespace Lp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello,World!");
            //.GetType("LP.Object.LpObject")

            //Type t = Type.GetType("LP.Object.LpObject");
            Type t = Type.GetType("LP.Object.LpObject");
            Console.WriteLine("hoge");
            Console.WriteLine( t.GetMethod("hoge", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null,null) );
            //t.GetType().InvokeMember(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null, null);
            //t.GetType().InvokeMember( "hoge",BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null, null );
            /*
            var field = type.GetField("hoge2", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);
            Console.WriteLine( field );
            Console.WriteLine( field.GetValue(type) );

            var field2 = type.GetField("hoge2", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);
                        Console.WriteLine("methods");
                        System.Reflection.MethodInfo[] methodInfo = type.GetMethods();
                        foreach (System.Reflection.MethodInfo mInfo in methodInfo)
                        {
                            Console.WriteLine(mInfo.ToString());
                        }
                        Object o = Activator.CreateInstance(type);
                        Console.WriteLine(o.GetType().InvokeMember("fuga", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
                        */
            /*
            LP.Object.LpObject o = new LP.Object.LpObject();
            Console.WriteLine(o.GetType().InvokeMember("fuga", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            Console.WriteLine(o.GetType().InvokeMember("puyo", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, o, null));
            int result = (int)o.GetType().InvokeMember("SumWithState", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, new object[] { 10 });
            Console.WriteLine(result);
             */
        }
    }
}
