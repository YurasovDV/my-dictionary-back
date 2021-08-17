using System.Diagnostics;

namespace DictionaryBack.BL.Query.Models
{
    [DebuggerDisplay("{Term}=[{Translations}]")]
    public class WordDto
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translations { get; set; }
    }
}
