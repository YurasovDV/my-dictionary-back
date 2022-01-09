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
            var dictionaryContext = ContextFactory.CreateDbContext(null);
            var dapperFacade = new DapperPgFacade(Configuration);
            var opts = Options.Create(new DictionaryApiSettings() { MaxWordsInRequest = 1000, RepetitionSetSize = 30 });

            var topicReadHandler = new WordsPagedQueryHandler(dictionaryContext, dapperFacade, new TranslationService(), opts, null);
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

            var topicReadHandler = new WordsPagedQueryHandler(dictionaryContext, dapperFacade, new TranslationService(), opts, null);
            var page = await topicReadHandler.GetPageDapperAsync(r);
            Console.WriteLine(page.StatusCode);
        }
    }
}
