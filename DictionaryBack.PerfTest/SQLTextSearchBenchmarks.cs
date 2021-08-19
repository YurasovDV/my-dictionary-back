using BenchmarkDotNet.Attributes;
using DictionaryBack.BL.Query;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using DictionaryBack.Infrastructure.Requests;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.PerfTest
{
    public class SQLTextSearchBenchmarks
    {
        public static IEnumerable<WordsByTopicRequest> GetRequests()
        {
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "for", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "sta", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "orm"};
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "orm", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "exc", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "pog", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "ter", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "his", Topic = "def" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "his", Topic = "111" };
            yield return new WordsByTopicRequest() { Skip = 0, Take = 200, SearchTerm = "drft", Topic = "def" };
        }

        private readonly static DesignDictionaryContextFactory ContextFactory;
        private readonly static IConfiguration Configuration;

        static SQLTextSearchBenchmarks()
        {
            ContextFactory = new DesignDictionaryContextFactory();

            var inMemorySettings = new Dictionary<string, string> {
                {"ConnectionStrings:WordsContext", "Host=localhost;Database=dict;Username=postgres;Password=1221"},
            };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetRequests))]
        public async Task TestEF(WordsByTopicRequest r)
        {
            var ctx = ContextFactory.CreateDbContext(null);
            var dapp = new DapperPgFacade(Configuration);

            var topicReadHandler = new WordsByTopicQueryHandler(ctx, dapp);
            var a = await topicReadHandler.GetPageNoTrackingAsync(r);
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetRequests))]
        public async Task TestDapper(WordsByTopicRequest r)
        {
            var ctx = ContextFactory.CreateDbContext(null);
            var dapp = new DapperPgFacade(Configuration);

            var topicReadHandler = new WordsByTopicQueryHandler(ctx, dapp);
            var a = await topicReadHandler.GetPageDapperAsync(r);
        }
    }
}
