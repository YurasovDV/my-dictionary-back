namespace DictionaryBack.Messages
{
    public interface WordMessage
    {
        public string Term { get; set; }

        public bool IsDeleted { get; set; }
    }
}
