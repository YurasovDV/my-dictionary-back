using DictionaryBack.Common.DTOs.Command;

namespace DictionaryBack.Messages
{
    public interface RepetitionEndedMessage
    {
        public WordRepetitionResult[] WordsRepetitionResults { get; set; }
    }
}
