using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.PerfTest
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            //await new SQLBenchmarks().TestDapper();
            //await new SQLBenchmarks().TestEF();


            //Summary summary = BenchmarkRunner.Run<SQLNoTextSearchBenchmarks>();
            // database caches work AMAZINGLY well so this benchmark is pointless without restarting PG server
            Summary summary = BenchmarkRunner.Run<SQLTextSearchBenchmarks>();
            Console.WriteLine("END");
            Console.ReadLine();
            return await Task.FromResult(0);
        }
    }
}
