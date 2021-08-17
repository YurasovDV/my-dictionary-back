using DictionaryBack.API;
using DictionaryBack.BL.Query.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DictionaryBack.Tests
{
    [TestClass]
    public class ReadSmoke
    {

        private static WordsByTopicRequest GetRequest() => new WordsByTopicRequest()
        {
            Skip = 0,
            Take = 20,
            Topic = null
        };

        [TestMethod]
        public async Task ReadPage()
        {
            using var factory = new WebApplicationFactory<Startup>();
            using var client = factory.CreateClient();
            var request = JsonSerializer.Serialize(GetRequest());
            var response = await client.PostAsync("DictionaryRead/GetPageNoTracking", new StringContent(request, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json));

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var words = JsonSerializer.Deserialize<List<WordDto>>(content, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            Assert.AreEqual(20, words.Count);
            Assert.IsTrue(words.All(w => !string.IsNullOrEmpty(w.Term)));
            Assert.IsTrue(words.All(w => w.Translations.Length >= 1));
            foreach (var word in words)
            {
                Assert.IsTrue(word.Translations.All(t => !string.IsNullOrEmpty(t)));
            }
        }
    }
}
