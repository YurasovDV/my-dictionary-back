using DictionaryBack.Common.Enums;

namespace DictionaryBack.Common.DTOs.Command
{
    public class WordEditModel
    {
        public WordStatus Status { get; set; }

        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translations { get; set; }
    }
}
