namespace DictionaryBack.BL.Query.Models
{
    public class WordsByTopicRequest
    {
        public string Topic { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
