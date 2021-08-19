using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.PerfTest
{
    class Program
    {
        static async Task<int> Main(string[] _)
        {
            //Summary summary = BenchmarkRunner.Run<SQLNoTextSearchBenchmarks>();
            // database caches work AMAZINGLY well so this benchmark is pointless without restarting PG server
            Summary summary = BenchmarkRunner.Run<SQLTextSearchBenchmarks>();
            Console.WriteLine($"END ({summary.TotalTime})");
            Console.ReadLine();
            return await Task.FromResult(0);
        }
    }
}
