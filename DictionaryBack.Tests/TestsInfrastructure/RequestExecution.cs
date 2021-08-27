using System;
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

        public static async Task<TRes> ExecutePostRequest<TRes>(HttpClient client, object request, string url)
        {
            var requestContent = GetContent(request);
            var responseMsg = await client.PostAsync(url, requestContent);
            return await HandleResponse<TRes>(responseMsg);
        }

        public static async Task<TRes> ExecutePostRequestWithoutBody<TRes>(HttpClient client, string url)
        {
            var @null = new ReadOnlyMemoryContent(ReadOnlyMemory<byte>.Empty);
            var responseMsg = await client.PostAsync(url, @null);
            return await HandleResponse<TRes>(responseMsg);
        }

        public static async Task<TRes> ExecutePutRequest<TRes>(HttpClient client, object request, string url)
        {
            var requestContent = GetContent(request);
            var responseMsg = await client.PutAsync(url, requestContent);
            return await HandleResponse<TRes>(responseMsg);
        }

        public static async Task<TRes> HandleResponse<TRes>(HttpResponseMessage responseMsg)
        {
            responseMsg.EnsureSuccessStatusCode();
            var content = await responseMsg.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TRes>(content, defaultOptions);
            return responseData;
        }
    }
}
