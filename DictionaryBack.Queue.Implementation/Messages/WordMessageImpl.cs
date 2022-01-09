using DictionaryBack.Messages;

namespace DictionaryBack.Queue.Implementation.Messages
{
    internal class WordMessageImpl : WordMessage
    {
        public string Term { get; set; }

        public bool IsDeleted { get; set; }
    }
}
