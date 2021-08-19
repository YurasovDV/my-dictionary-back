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
    public class SQLNoTextSearchBenchmarks
    {
        public static IEnumerable<WordsByTopicRequest> GetRequests()
        {
            yield return new WordsByTopicRequest() { Skip = 0, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 600, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 900, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 300, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 1600, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 5600, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 8600, Take = 400 };
            yield return new WordsByTopicRequest() { Skip = 3000, Take = 400 };
        }

        private readonly static DesignDictionaryContextFactory ContextFactory;
        private readonly static IConfiguration Configuration;

        static SQLNoTextSearchBenchmarks()
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
