using DictionaryBack.API;
using DictionaryBack.Infrastructure;
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
using DictionaryBack.Tests.TestsInfrastructure;
using DictionaryBack.Infrastructure.DTOs.Query;
using DictionaryBack.Domain;
using DictionaryBack.API;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.DTOs.Query;
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
    public class RepetitionSet
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
        public async Task CreateSet()
        {
            var resp = await RequestExecution.ExecutePostRequestWithoutBody<OperationResult<WordDto[]>>(client, Urls.Repetition.CreateRepetitionSet);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }

            Assert.AreEqual(Constants.RepetitionSetSize, resp.Data.Length);
        }


       // [TestMethod]
        public async Task PerformRepetition()
        {

            var resp = await RequestExecution.ExecutePostRequestWithoutBody<OperationResult<WordDto[]>>(client, Urls.Repetition.CreateRepetitionSet);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            Assert.AreEqual(Constants.RepetitionSetSize, resp.Data.Length);



        }

    }
}
