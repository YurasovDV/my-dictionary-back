namespace DictionaryBack.BL.Query.Models
{
    public class WordDto
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translation { get; set; }
    }
}
