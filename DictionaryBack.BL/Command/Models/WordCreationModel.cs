﻿namespace DictionaryBack.BL.Command.Models
{
    public class WordCreationModel
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translation { get; set; }
    }
}
