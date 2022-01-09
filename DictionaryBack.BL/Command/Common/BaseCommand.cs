using DictionaryBack.Common.Localization;
using DictionaryBack.Common.Queue;
using DictionaryBack.DAL;

namespace DictionaryBack.BL.Command
{
    public class BaseCommand 
    {
        protected readonly DictionaryContext DictionaryContext;
        protected readonly ITranslationService TranslationService;
        protected readonly TopicFinder TopicFinder;
        protected readonly IWordsPublisher WordsPublisher;

        public BaseCommand(DictionaryContext dictionaryContext, ITranslationService translationService, IWordsPublisher wordsPublisher)
        {
            DictionaryContext = dictionaryContext;
            TranslationService = translationService;
            WordsPublisher = wordsPublisher;
            TopicFinder = new TopicFinder(dictionaryContext, translationService);
        }
    }
}
