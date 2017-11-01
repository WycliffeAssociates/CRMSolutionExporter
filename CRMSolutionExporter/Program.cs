using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Crm.Sdk.Messages;
using System.IO;

namespace CRMSolutionExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Usage();
                return;
            }

            string connectionString = args[0];
            string solutionName = args[1];
            string fileName = args[2];
            string version = args[3];

            CrmServiceClient service = new CrmServiceClient(connectionString);

            Console.WriteLine("Publishing");
            PublishAllXmlRequest publishRequest = new PublishAllXmlRequest();
            service.Execute(publishRequest);

            Console.WriteLine("Exporting");
            ExportSolutionRequest request = new ExportSolutionRequest()
            {
                SolutionName = solutionName,
                Managed = false,
                TargetVersion = version
            };

            ExportSolutionResponse response = (ExportSolutionResponse)service.Execute(request);

            Console.WriteLine("Writing");
            File.WriteAllBytes(fileName, response.ExportSolutionFile);

        }

        private static void Usage()
        {
            Console.WriteLine("Usage CRMSolutionExporter.exe <connection-string> <solution-name> <resulting-file> <target-version>");
        }
    }
}
