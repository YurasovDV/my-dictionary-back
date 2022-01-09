using DictionaryBack.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DictionaryBack.Tests.TestsInfrastructure;
using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Query;

namespace DictionaryBack.Tests
{
    [TestClass]
    [TestCategory("Command")]
    public class DeleteWord
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
        public async Task DeleteWordSuccess()
        {
            var request = Requests.Query.GetRequestForFirst20WordsWith_Def_Topic();
            request.SearchTerm = "for";
            var getPageResponse = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, Urls.Query.GetPageNoTracking);
            if (!getPageResponse.IsSuccessful())
            {
                Assert.Fail(getPageResponse.ErrorText);
            }
            WordDto[] words = getPageResponse.Data.Page;

            Assert.AreEqual(20, words.Length);

            var prey = words.First();

            // act
            var resp = await client.DeleteAsync(Urls.Command.DeleteWord + prey.Term);
            var deleteStatus = await RequestExecution.HandleResponse<BoolOperationResult>(resp);

            // assert
            if (!deleteStatus.IsSuccessful())
            {
                Assert.Fail(deleteStatus.ErrorText);
            }

            var getPageResponse2 = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, Urls.Query.GetPageNoTracking);
            if (!getPageResponse2.IsSuccessful())
            {
                Assert.Fail(getPageResponse.ErrorText);
            }

            Assert.IsFalse(getPageResponse2.Data.Page.Any(w => w.Term.Equals(prey.Term, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
