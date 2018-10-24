using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using System.IO;
using Microsoft.Xrm.Sdk.Query;

namespace CRMSolutionExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineArguments arguments = new CommandLineArguments();
            if (!CommandLine.Parser.Default.ParseArguments(args, arguments))
            {
                Environment.Exit(1);
            }

            CrmServiceClient service = new CrmServiceClient(arguments.ConnectionString);
            if (!service.IsReady)
            {
                Console.WriteLine("Error connecting to CRM environment please check connection string");
                Environment.Exit(2);
            }

            // Check solution
            Console.WriteLine("Checking solution");
            QueryExpression query = new QueryExpression("solution");
            query.ColumnSet = new ColumnSet("uniquename", "friendlyname", "version");
            query.Criteria.AddCondition("uniquename", ConditionOperator.Equal, arguments.SolutionName);
            var result = service.RetrieveMultiple(query);

            if (result.Entities.Count == 0)
            {
                Console.WriteLine("Solution with the unique name " + arguments.SolutionName + " does not exist");
                Environment.Exit(3);
            }

            string friendlyName = (string)result.Entities[0]["friendlyname"];
            string version = (string)result.Entities[0]["version"];


            Console.WriteLine("Publishing");
            PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
            service.Execute(publishRequest);

            //Figure out the file name for the resulting file
            string fileName = arguments.FileName;
            if (fileName == null)
            {
                fileName = arguments.SolutionName + version.Replace('.', '_') + ".zip";
            }

            Console.WriteLine("Exporting");
            Console.WriteLine("Solution: " + friendlyName + " version: " + version);
            ExportSolutionRequest request = new ExportSolutionRequest()
            {
                SolutionName = arguments.SolutionName,
                Managed = arguments.Managed,
            };
            if (!string.IsNullOrEmpty(arguments.TargetVersion))
            {
                request.TargetVersion = arguments.TargetVersion;
            }

            ExportSolutionResponse response = (ExportSolutionResponse)service.Execute(request);

            Console.WriteLine("Writing");
            File.WriteAllBytes(fileName, response.ExportSolutionFile);

        }
    }
}
