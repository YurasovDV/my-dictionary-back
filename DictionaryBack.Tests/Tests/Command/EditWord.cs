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
    public class EditWord
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
        public async Task EditWordSuccess()
        {
            var request = Requests.Query.GetRequestForFirstKWords();

            var getPageResponse = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, request, Urls.Query.GetPageNoTracking);
            if (!getPageResponse.IsSuccessful())
            {
                Assert.Fail(getPageResponse.ErrorText);
            }
            WordDto[] words = getPageResponse.Data.Page;

            Assert.AreEqual(20, words.Length);

            var prey = words.First();

            // prepare
            var editRequest = Requests.Command.Edit.CopyOf(prey);
            editRequest.Translations =
                editRequest.Translations
                    .Concat(new string[] { Guid.NewGuid().ToString() })
                    .ToArray();

            var editwordResponse = await RequestExecution.ExecutePutRequest<OperationResult<WordDto>>(client, editRequest, Urls.Command.EditWord);
            if (!editwordResponse.IsSuccessful())
            {
                Assert.Fail(editwordResponse.ErrorText);
            }

            Assert.AreEqual(editRequest.Term, editwordResponse.Data.Term, ignoreCase: true);
            Assert.AreEqual(editRequest.Translations.Length, editwordResponse.Data.Translations.Length);

            CollectionAssert.AreEqual(editRequest.Translations, editwordResponse.Data.Translations, StringComparer.OrdinalIgnoreCase);
        }
    }
}
