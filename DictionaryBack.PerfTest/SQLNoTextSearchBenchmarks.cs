using BenchmarkDotNet.Attributes;
using DictionaryBack.BL.Query;
using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using DictionaryBack.ErrorMessages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
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
            var dictionaryContext = ContextFactory.CreateDbContext(null);
            var dapperFacade = new DapperPgFacade(Configuration);
            var opts = Options.Create(new DictionaryApiSettings() { MaxWordsInRequest = 1000, RepetitionSetSize = 30 });

            var topicReadHandler = new WordsPagedQueryHandler(dictionaryContext, dapperFacade, new TranslationService(), opts);
            var page = await topicReadHandler.GetPageNoTrackingAsync(r);
            Console.WriteLine(page.StatusCode);
        }

        [Benchmark]
        [ArgumentsSource(nameof(GetRequests))]
        public async Task TestDapper(WordsByTopicRequest r)
        {
            var dictionaryContext = ContextFactory.CreateDbContext(null);
            var dapperFacade = new DapperPgFacade(Configuration);

            var opts = Options.Create(new DictionaryApiSettings() { MaxWordsInRequest = 1000, RepetitionSetSize = 30 });

            var topicReadHandler = new WordsPagedQueryHandler(dictionaryContext, dapperFacade, new TranslationService(), opts);
            var page = await topicReadHandler.GetPageDapperAsync(r);
            Console.WriteLine(page.StatusCode);
        }
    }
}
