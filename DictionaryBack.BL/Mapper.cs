using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Domain;
using System;
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
                Topic = model.Topic, 
                Translations = Map(model.Term, model.Translations)
            };
        }

        public static Word Map(WordEditModel model)
        {
            return new Word
            {
                Term = model.Term,
                Topic = model.Topic,
                Translations = Map(model.Term, model.Translations)
            };
        }

        public static WordDto Map(Word model)
        {
            return new WordDto() { Term = model.Term, Topic = model.Topic, Translations = Map(model.Translations) };
        }

        private static string[] Map(ICollection<Translation> translations)
        {
            return translations.Select(t => t.Meaning).ToArray();
        }

        private static Translation[] Map(string term, string[] translations)
        {
            return translations.Select(t => new Translation() { Meaning = t, Term = term }).ToArray();
        }
    }
}
