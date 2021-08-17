namespace DictionaryBack.Infrastructure.Requests
{
    public class WordsByTopicRequest
    {
        public string SearchTerm { get; set; }

        public string Topic { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
