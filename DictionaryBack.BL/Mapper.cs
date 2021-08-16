using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Domain;

namespace DictionaryBack.BL
{
    internal static class Mapper
    {
        public static Word Map(WordCreationModel model) => new() { Term = model.Term, Topic = model.Topic, Translations = model.Translation };

        public static Word Map(WordEditModel model) => new() { Term = model.Term, Topic = model.Topic, Translations = model.Translations };

        public static WordDto Map(Word model) => new() { Term = model.Term, Topic = model.Topic, Translation = model.Translations };
    }
}
