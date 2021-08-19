using DictionaryBack.API;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Infrastructure;
using DictionaryBack.Tests.TestsInfrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DictionaryBack.Tests
{
    [TestClass]
    [TestCategory("Command")]
    public class AddWord
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

        [TestMethod]
        public async Task AddWordSmokeSuccess()
        {
            var request = Requests.Command.AddWordRequest();

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<WordDto>>(client, request, Urls.Command.AddWord);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }

            Assert.AreEqual(request.Term, resp.Data.Term, ignoreCase: true);
            Assert.AreEqual(request.Translations.Length, resp.Data.Translations.Length);
            Assert.AreEqual(request.Translations[0], resp.Data.Translations[0], ignoreCase: true);
        }

        [TestMethod]
        public async Task AddDuplicatedWordFails()
        {
            var request = Requests.Command.AddWordRequestDuplicated();

            var resp = await RequestExecution.ExecutePostRequest<OperationResult<List<WordDto>>>(client, request, Urls.Command.AddWord);

            Assert.IsFalse(resp.IsSuccessful());
            Console.WriteLine(resp.ErrorText);
        }
    }
}
