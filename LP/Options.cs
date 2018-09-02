using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace LP
{
    class Options
    {
        string script = null;

        [Option('v', null, HelpText = "Print details during execution.")]
        public bool Verbose
        {
            get;
            set;
        }

        [Option('e', null, HelpText="evaluate ruby script.")]
        public string Evaluate
        {
            get {
                return this.script;
            }
            set {
                if(value.Length!=0)
                    this.script = value;
            }
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
