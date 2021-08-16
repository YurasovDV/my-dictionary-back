namespace DictionaryBack.Domain
{
    // TODO: separate model for persistence layer?
    public class Word
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translation { get; set; }
    }
}
