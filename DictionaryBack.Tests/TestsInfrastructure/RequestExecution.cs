using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DictionaryBack.Tests.TestsInfrastructure
{
    public static class RequestExecution
    {
        private static readonly JsonSerializerOptions defaultOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


        private static StringContent GetContent(object request) => 
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

        public static async Task<TRes> ExecuteRequest<TRes>(HttpClient client, object request, string url)
        {
            var requestContent = GetContent(request);
            var responseMsg = await client.PostAsync(url, requestContent);
            responseMsg.EnsureSuccessStatusCode();
            var content = await responseMsg.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TRes>(content, defaultOptions);
            return responseData;
        }
    }
}
