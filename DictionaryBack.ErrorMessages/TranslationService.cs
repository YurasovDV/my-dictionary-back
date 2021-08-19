using DictionaryBack.Infrastructure;
using System.Collections.Generic;

namespace DictionaryBack.ErrorMessages
{
    public class TranslationService : ITranslationService
    {
        public string GetTranslation(string key)
        {
            return _translations[key];
        }

        private Dictionary<string, string> _translations = new()
        {
            { "Topic not found", "Topic not found" },
            { "No word provided", "No word provided" },
            { "Incorrect translations", "Incorrect translations" },
            { "Incorrect topic name", "Incorrect topic name" },
            { "Word already exists", "Word already exists" },
            { "Internal error", "Internal error" },
            { "Word does not exist", "Word does not exist" },
            { "Not implemented", "Not implemented" },
            { "Too many items requested" , "Too many items requested" },
        };
    }
}
