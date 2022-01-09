using DictionaryBack.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DictionaryBack.Tests.TestsInfrastructure;
using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.Enums;

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

            Assert.AreEqual(30, resp.Data.Length);
        }


        [TestMethod]
        public async Task PerformRepetition()
        {
            // prepare
            var resp = await RequestExecution.ExecutePostRequestWithoutBody<OperationResult<WordDto[]>>(client, Urls.Repetition.CreateRepetitionSet);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }
            Assert.AreEqual(30, resp.Data.Length);

            // act

            var trainingResult = resp.Data
                .Select(w => 
                    new WordRepetitionResult() 
                    {  
                        RepetitionStatus = RepetitionStatus.Success, 
                        Term = w.Term 
                    })
                .ToArray();

            var last = trainingResult.Last();
            last.RepetitionStatus = RepetitionStatus.FailedMultipleTimes;

            var completionResult = await RequestExecution.ExecutePostRequest<BoolOperationResult>(client, trainingResult, Urls.Repetition.CompleteRepetition);
            if (!resp.IsSuccessful())
            {
                Assert.Fail(resp.ErrorText);
            }


            // assert
            var getRequest = Requests.Query.GetRequestForFirstKWords(1000);
            getRequest.Topic = null;
            getRequest.SearchTerm = last.Term;
            var wordsFound = await RequestExecution.ExecutePostRequest<OperationResult<PageData<WordDto>>>(client, getRequest, Urls.Query.GetPageNoTracking);

            if (!wordsFound.IsSuccessful())
            {
                Assert.Fail(wordsFound.ErrorText);
            }

            // todo may be flaky
            var read = wordsFound.Data.Page.FirstOrDefault(w => w.Term.Equals(last.Term, StringComparison.OrdinalIgnoreCase));
            if (read == null)
            {
                Assert.Fail("no word found");
            }

            Assert.AreEqual(last.RepetitionStatus, read.RepetitionStatus);

        }

    }
}
