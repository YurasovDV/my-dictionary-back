﻿namespace DictionaryBack.Domain
{
    public class Word
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translations { get; set; }

        public bool IsDeleted { get; set; }
    }
}
