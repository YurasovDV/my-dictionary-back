using DictionaryBack.Common.Enums;

namespace DictionaryBack.Common.DTOs.Command
{
    public class WordRepetitionResult
    {
        public string Term { get; set; }

        public RepetitionStatus RepetitionStatus { get; set; }

        public override string ToString()
        {
            return $"{Term} - {RepetitionStatus}";
        }
    }
}
