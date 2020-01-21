using CaseManagement.CMMN;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CaseManagement.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start CaseManagement engine");
            var serviceCollection = new ServiceCollection();
            var caseJobServer = new CaseJobServer(serviceCollection);
            caseJobServer.Start();
            Console.WriteLine("Press a key to quit");
            caseJobServer.Stop();
            Console.ReadKey();
        }
    }
}
