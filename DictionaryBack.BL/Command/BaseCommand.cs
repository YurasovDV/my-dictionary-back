using DictionaryBack.BL.Command.Common;
using DictionaryBack.DAL;

namespace DictionaryBack.BL.Command
{
    public class BaseCommand 
    {
        protected readonly DictionaryContext DictionaryContext;

        protected readonly TopicFinder TopicFinder;

        public BaseCommand(DictionaryContext dictionaryContext)
        {
            DictionaryContext = dictionaryContext;
            TopicFinder = new TopicFinder(dictionaryContext);
        }
    }
}
