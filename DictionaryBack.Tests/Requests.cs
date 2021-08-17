using DictionaryBack.Infrastructure.Requests;

namespace DictionaryBack.Tests
{
    internal static class Requests
    {
        internal static WordsByTopicRequest GetRequestForFirstKWords(int take = 20) => new WordsByTopicRequest()
        {
            Skip = 0,
            Take = take,
            SearchTerm = null,
            Topic = null,
        };

        internal static WordsByTopicRequest GetRequestForFirst20WordsWith_For_Query() => new WordsByTopicRequest()
        {
            Skip = 0,
            Take = 20,
            SearchTerm = "for",
            Topic = null
        };
    }
}
