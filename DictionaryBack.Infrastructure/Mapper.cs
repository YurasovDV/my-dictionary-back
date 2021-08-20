using DictionaryBack.Domain;
using DictionaryBack.Infrastructure.DTOs.Command;
using DictionaryBack.Infrastructure.DTOs.Query;
using System.Collections.Generic;
using System.Linq;

namespace DictionaryBack.Infrastructure
{
    public static class Mapper
    {
        public static Word Map(WordCreationModel model)
        {
            return new Word
            {
                Term = model.Term,
                Topic = Map(model.Topic),
                Translations = Map(model.Term, model.Translations),
                Status = WordStatus.Added
            };
        }

        public static WordDto Map(Word model)
        {
            return new WordDto()
            {
                Term = model.Term,
                Topic = model.Topic?.Name,
                Translations = Map(model.Translations),
                Status = model.Status,
                LastRepetition = model.LastRepetition,
                RepetitionStatus = model.RepetitionStatus,
            };
        }

        private static string[] Map(ICollection<Translation> translations)
        {
            return translations.Select(t => t.Meaning).ToArray();
        }

        private static Translation[] Map(string term, string[] translations)
        {
            return translations.Select(t => new Translation() { Meaning = t, Term = term }).ToArray();
        }

        private static Topic Map(string topic)
        {
            return new Topic()
            {
                Name = topic,
            };
        }
    }
}
