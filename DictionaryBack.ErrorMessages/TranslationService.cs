using DictionaryBack.Infrastructure;
using System.Collections.Generic;

namespace DictionaryBack.ErrorMessages
{
    public class TranslationService : ITranslationService
    {
        private readonly Dictionary<ErrorKey, string> _translations2 = new()
        {
            { ErrorKey.TopicNotFound, "Topic not found" },
            { ErrorKey.NoWordProvided, "No word provided" },
            { ErrorKey.InvalidTranslations, "Incorrect translations" },
            { ErrorKey.InvalidTopicName, "Incorrect topic name" },
            { ErrorKey.WordAlreadyExists, "Word already exists in database(was it deleted?)" },
            { ErrorKey.InternalError, "Internal error" },
            { ErrorKey.WordDoesNotExist, "Word does not exist" },
            { ErrorKey.NotImplemented, "Functionality is not implemented" },
            { ErrorKey.TooManyItemsRequested, "Too many items requested" },
            { ErrorKey.NoWordsForRepetion, "No words for repetion" },
        };

        public string GetTranslation(ErrorKey key)
        {
            return _translations2[key];
        }
    }
}
