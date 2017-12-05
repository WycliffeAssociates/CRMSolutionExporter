using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace CRMSolutionExporter
{
    public class CommandLineArguments
    {
        [Option('c',"connectionstring", Required = true, HelpText = "Connection string for the target CRM environment")]
        public string ConnectionString { get; set; }
        [Option("solution", Required = true, HelpText = "Unique name of the solution to export")]
        public string SolutionName { get; set; }
        [Option("file", HelpText = "Name of the resulting file. If not supplied a name will be generated for you")]
        public string FileName { get; set; }
        [Option("target", Required = true, HelpText = "The target version of CRM to export for")]
        public string TargetVersion { get; set; }
        [Option("managed", DefaultValue = false, HelpText ="Whether or not the solution should be exported as managed")]
        public bool Managed { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
