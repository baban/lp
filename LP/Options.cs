using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace LP
{
    class Options
    {
        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose
        {
            get;
            set;
        }

        [Option('e', null, HelpText="evaluate ruby script.")]
        public string Evaluate
        {
            get;
            set;
        }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("LP", (0.1).ToString() ),
                Copyright = new CopyrightInfo("baban", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddOptions(this);
            return help;
        }

        [ValueList(typeof(List<string>))]
        public IList<string> InputFiles
        {
            get;
            set;
        }
    }
}
