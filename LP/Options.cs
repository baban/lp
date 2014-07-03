using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP
{
    class Options
    {
        [CommandLine.Option('v',"version")]
        public bool Version
        {
            get {
                Console.WriteLine("LP version {0}", 0.1);
                return true;
            }
            set {
            }
        }

        [CommandLine.Option('e', "evaluate")]
        public bool Evaluate
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        [CommandLine.Option('h', "help")]
        public bool Help
        {
            get
            {
                Console.WriteLine("LP version {0}", 0.1);
                Console.WriteLine("-h --help print help ");
                Console.WriteLine("-v --version print version info ");
                Console.WriteLine("-e --evalueate evaluate script ");
                return true;
            }
            set
            {
            }
        }

    }
}
