using DictionaryBack.Infrastructure.Requests;

namespace DictionaryBack.Tests.TestsInfrastructure
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

        internal static WordsByTopicRequest GetRequestForFirst20WordsWith_For_Query(int take = 20) => new WordsByTopicRequest()
        {
            Skip = 0,
            Take = take,
            SearchTerm = "for",
            Topic = null
        };

        internal static WordsByTopicRequest GetRequestForFirst20WordsWith_For_Query_Def_Topic(int take = 20) => new WordsByTopicRequest()
        {
            Skip = 0,
            Take = take,
            SearchTerm = "for",
            Topic = "def"
        };

        internal static WordsByTopicRequest GetRequestForFirst20WordsWith_Def_Topic(int take = 20) => new WordsByTopicRequest()
        {
            Skip = 0,
            Take = take,
            SearchTerm = null,
            Topic = "def"
        };
    }
}
