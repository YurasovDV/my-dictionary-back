namespace DictionaryBack.Infrastructure.DTOs.Query
{
    public class WordsByTopicRequest
    {
        public string SearchTerm { get; set; }

        public string Topic { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        /// <summary>
        /// for benchmarks
        /// </summary>
        public override string ToString()
        {
            return $"{Skip}-{Skip + Take},T={Topic},Q={SearchTerm}";
        }
    }
}
