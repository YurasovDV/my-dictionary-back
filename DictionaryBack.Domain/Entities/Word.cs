using System;
using System.Collections.Generic;

namespace DictionaryBack.Domain
{
    public class Word
    {
        public string Term { get; set; }

        public ICollection<Translation> Translations { get; set; }

        public Topic Topic { get; set; }

        public int TopicId { get; set; }

        public WordStatus Status { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? LastRepetition { get; set; }

        public RepetitionStatus RepetitionStatus { get; set; }
    }
}
