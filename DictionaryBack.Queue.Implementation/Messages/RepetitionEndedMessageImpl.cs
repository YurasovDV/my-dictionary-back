using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Messages;

namespace DictionaryBack.Queue.Implementation.Messages
{
    internal class RepetitionEndedMessageImpl : RepetitionEndedMessage
    {
        public WordRepetitionResult[] WordsRepetitionResults { get; set; }
    }
}
