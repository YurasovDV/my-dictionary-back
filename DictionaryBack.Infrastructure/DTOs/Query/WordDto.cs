using DictionaryBack.Domain;
using System;
using System.Linq;

namespace DictionaryBack.Infrastructure.DTOs.Query
{
    // nq = no quotes
    [System.Diagnostics.DebuggerDisplay("DebuggerDisplay,nq")]
    public class WordDto
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translations { get; set; }

        public WordStatus Status { get; set; }

        public RepetitionStatus RepetitionStatus { get; set; }
        public DateTime? LastRepetition { get; set; }

        private string DebuggerDisplay
        {
            get => $"{Term} -> {Translations.Aggregate(string.Empty, (acc, t) => $"{acc}, {t}")}";
        }
    }
}
