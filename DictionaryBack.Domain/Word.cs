using System;
using System.Collections.Generic;

namespace DictionaryBack.Domain
{
    public class Word
    {
        public string Term { get; set; }

        public Topic Topic { get; set; }

        public int TopicId { get; set; }

        public ICollection<Translation> Translations { get; set; }

        public bool IsDeleted { get; set; }

        public WordStatus Status { get; set; }

        public DateTime? LastRepetition { get; set; }
    }
}
