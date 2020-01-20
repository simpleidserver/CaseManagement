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
            using (var caseJobServer = new CaseJobServer(s =>
            {
                s.AddLogging(c => c.AddConsole());
            }))
            {
                Console.WriteLine("Press a key to quit");
                Console.ReadKey();
            }
        }
    }
}
