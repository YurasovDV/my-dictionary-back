using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Domain;
using System.Collections.Generic;
using System.Linq;

namespace DictionaryBack.BL
{
    internal static class Mapper
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

        /*public static Word Map(WordEditModel model)
        {
            return new Word
            {
                Term = model.Term,
                Topic = Map(model.Topic),
                Translations = Map(model.Term, model.Translations),
                Status = model.Status,
            };
        }*/

        public static WordDto Map(Word model)
        {
            return new WordDto() 
            { 
                Term = model.Term, 
                Topic = model.Topic?.Name, 
                Translations = Map(model.Translations),
                Status = model.Status,
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
