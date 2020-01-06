using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;
using System;
using System.Linq;

namespace CaseManagement.CMMN.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new BenchmarkSwitcher(typeof(Program).Assembly).Run(args, new Config());
            Console.ReadLine();
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                Add(MemoryDiagnoser.Default);
                Add(MarkdownExporter.GitHub);
                Add(DefaultConfig.Instance.GetLoggers().ToArray());
                Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
            }
        }
    }
}
