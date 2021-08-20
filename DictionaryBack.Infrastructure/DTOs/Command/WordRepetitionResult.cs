using DictionaryBack.Domain;

namespace DictionaryBack.Infrastructure.DTOs.Command
{
    public class WordRepetitionResult
    {
        public string Term { get; set; }

        public RepetitionStatus RepetitionStatus { get; set; }
    }
}
