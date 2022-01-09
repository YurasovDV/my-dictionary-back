using DictionaryBack.API;
using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Tests.TestsInfrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DictionaryBack.Tests
{
    [TestClass]
    [TestCategory("Query")]
    public class ReadSmoke
    {
        private static WebApplicationFactory<Startup> factory;
        private static HttpClient client;

        [ClassInitialize]
        public static void Init(TestContext _)
        {
            factory = new WebApplicationFactory<Startup>();
            client = factory.CreateClient();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            client.Dispose();
            factory.Dispose();
        }

        [DataTestMethod]
        [DataRow(Urls.Query.GetPage)]
        [DataRow(Urls.Query.GetPageNoTracking)]
        [DataRow(Urls.Query.GetPageDapper)]
        public async Task ReadPage20(string url)
        {
            var request = Requests.Query.GetRequestForFirstKWords();

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, url);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            WordDto[] words = resp.Data.Page;

            Console.WriteLine($"{nameof(ReadPage20)} +   {url}: {resp.Data.Total}");
            Assert.IsTrue(resp.Data.Total > 0);

            Assert.AreEqual(20, words.Length);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }

        [DataTestMethod]
        [DataRow(Urls.Query.GetPage)]
        [DataRow(Urls.Query.GetPageNoTracking)]
        [DataRow(Urls.Query.GetPageDapper)]
        public async Task ReadPage100(string url)
        {
            var request = Requests.Query.GetRequestForFirstKWords(100);

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, url);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            WordDto[] words = resp.Data.Page;
            Console.WriteLine($"{nameof(ReadPage100)} +   {url}: {resp.Data.Total}");
            Assert.IsTrue(resp.Data.Total > 0);

            Assert.AreEqual(100, words.Length);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }


        [DataTestMethod]
        [DataRow(Urls.Query.GetPage)]
        [DataRow(Urls.Query.GetPageNoTracking)]
        [DataRow(Urls.Query.GetPageDapper)]
        public async Task ReadPageByTextSearch(string url)
        {
            var request = Requests.Query.GetRequestForFirst20WordsWith_For_Query();

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, url);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            WordDto[] words = resp.Data.Page;

            Console.WriteLine($"{nameof(ReadPageByTextSearch)} +   {url}: {resp.Data.Total}");
            Assert.IsTrue(resp.Data.Total > 0);

            Assert.AreEqual(20, words.Length);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Term.IndexOf("for", StringComparison.OrdinalIgnoreCase) != -1));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }

        [DataTestMethod]
        [DataRow(Urls.Query.GetPage)]
        [DataRow(Urls.Query.GetPageNoTracking)]
        [DataRow(Urls.Query.GetPageDapper)]
        public async Task ReadPageByTopicSearch(string url)
        {
            var request = Requests.Query.GetRequestForFirst20WordsWith_Def_Topic();

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, url);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            WordDto[] words = resp.Data.Page;

            Console.WriteLine($"{nameof(ReadPageByTopicSearch)} +   {url}: {resp.Data.Total}");
            Assert.IsTrue(resp.Data.Total > 0);

            Assert.AreEqual(20, words.Length);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Topic.IndexOf("def", StringComparison.OrdinalIgnoreCase) != -1));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }

        [DataTestMethod]
        [DataRow(Urls.Query.GetPage)]
        [DataRow(Urls.Query.GetPageNoTracking)]
        [DataRow(Urls.Query.GetPageDapper)]
        public async Task ReadPageByTopicAndTextSearch(string url)
        {
            var request = Requests.Query.GetRequestForFirst20WordsWith_For_Query_Def_Topic();

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, url);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            WordDto[] words = resp.Data.Page;

            Console.WriteLine($"{nameof(ReadPageByTopicAndTextSearch)} +   {url}: {resp.Data.Total}");
            Assert.IsTrue(resp.Data.Total > 0);

            Assert.AreEqual(20, words.Length);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Term.IndexOf("for", StringComparison.OrdinalIgnoreCase) != -1));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }
    }
}
