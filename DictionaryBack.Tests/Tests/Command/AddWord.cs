using DictionaryBack.API;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.Requests;
using DictionaryBack.Tests.TestsInfrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DictionaryBack.Tests
{
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

            var resp = await RequestExecution.ExecuteRequest<OperationResult<WordDto>>(client, request, Urls.Command.AddWord);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
        }

        [TestMethod]
        public async Task AddDuplicatedWordFails()
        {
            var request = Requests.Command.AddWordRequestDuplicated();

            var resp = await RequestExecution.ExecuteRequest<OperationResult<List<WordDto>>>(client, request, Urls.Command.AddWord);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
        }
    }
}
