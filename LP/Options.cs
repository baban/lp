using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP
{
    class Options
    {
        //Boolean型のオプション
        [CommandLine.Option('v')]
        public bool Overwrite
        {
            get {
                Console.WriteLine("LP version {0}", 0.1);
                return true;
            }
            set {
            }
        }
    }
}
