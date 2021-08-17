using DictionaryBack.API;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Infrastructure.Requests;
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
    [TestClass]
    public class ReadSmoke
    {
        [DataTestMethod]
        [DataRow("DictionaryRead/GetPage")] 
        [DataRow("DictionaryRead/GetPageNoTracking")] 
        [DataRow("DictionaryRead/GetPageDapper")] 
        public async Task ReadPage20(string url)
        {
            var request = Requests.GetRequestForFirstKWords();

            List<WordDto> words = await ExecuteRequest(request, url);

            Assert.AreEqual(20, words.Count);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }

        [DataTestMethod]
        [DataRow("DictionaryRead/GetPage")]
        [DataRow("DictionaryRead/GetPageNoTracking")]
        [DataRow("DictionaryRead/GetPageDapper")]
        public async Task ReadPage100(string url)
        {
            var request = Requests.GetRequestForFirstKWords(100);

            List<WordDto> words = await ExecuteRequest(request, url);

            Assert.AreEqual(100, words.Count);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }


        [DataTestMethod]
        [DataRow("DictionaryRead/GetPage")]
        [DataRow("DictionaryRead/GetPageNoTracking")]
        [DataRow("DictionaryRead/GetPageDapper")]
        public async Task ReadPageByTextSearch(string url)
        {
            var request = Requests.GetRequestForFirst20WordsWith_For_Query();

            List<WordDto> words = await ExecuteRequest(request, url);

            Assert.AreEqual(20, words.Count);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Term.IndexOf("for", StringComparison.OrdinalIgnoreCase) != -1));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }

        [DataTestMethod]
        [DataRow("DictionaryRead/GetPage")]
        [DataRow("DictionaryRead/GetPageNoTracking")]
        [DataRow("DictionaryRead/GetPageDapper")]
        public async Task ReadPageByTopicSearch(string url)
        {
            var request = Requests.GetRequestForFirst20WordsWith_Def_Topic();

            List<WordDto> words = await ExecuteRequest(request, url);

            Assert.AreEqual(20, words.Count);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Topic.IndexOf("def", StringComparison.OrdinalIgnoreCase) != -1));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }

        [DataTestMethod]
        [DataRow("DictionaryRead/GetPage")]
        [DataRow("DictionaryRead/GetPageNoTracking")]
        [DataRow("DictionaryRead/GetPageDapper")]
        public async Task ReadPageByTopicAndTextSearch(string url)
        {
            var request = Requests.GetRequestForFirst20WordsWith_For_Query_Def_Topic();

            List<WordDto> words = await ExecuteRequest(request, url);

            Assert.AreEqual(20, words.Count);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Term.IndexOf("for", StringComparison.OrdinalIgnoreCase) != -1));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }


        private async Task <List<WordDto>> ExecuteRequest(WordsByTopicRequest request, string url)
        {
            using var factory = new WebApplicationFactory<Startup>();
            using var client = factory.CreateClient();
            var requestContent = JsonSerializer.Serialize(request);
            var response = await client.PostAsync(url, new StringContent(requestContent, Encoding.UTF8, MediaTypeNames.Application.Json));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var words = JsonSerializer.Deserialize<List<WordDto>>(content, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return words;
        }
    }
}
