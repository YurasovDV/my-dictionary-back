namespace DictionaryBack.Contracts
{
    public interface WordMessage
    {
        public string Term { get; set; }

        public bool IsDeleted { get; set; }
    }
}
