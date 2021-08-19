using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;

namespace DictionaryBack.BL.Command
{
    public class BaseCommand 
    {
        protected readonly DictionaryContext DictionaryContext;
        protected readonly ITranslationService TranslationService;
        protected readonly TopicFinder TopicFinder;

        public BaseCommand(DictionaryContext dictionaryContext, ITranslationService translationService)
        {
            DictionaryContext = dictionaryContext;
            TranslationService = translationService;
            TopicFinder = new TopicFinder(dictionaryContext, translationService);
        }
    }
}
