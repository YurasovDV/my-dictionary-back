using DictionaryBack.DAL;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure;
using System;
using System.Linq;

namespace DictionaryBack.BL.Command.Common
{
    public class TopicFinder
    {
        protected DictionaryContext DictionaryContext { get; private set; }

        public TopicFinder(DictionaryContext dictionaryContext)
        {
            DictionaryContext = dictionaryContext ?? throw new ArgumentNullException(nameof(dictionaryContext));
        }

        internal OperationResult<Topic> FindTopic(string topicName)
        {
            var existing = DictionaryContext.Topics.FirstOrDefault(t => t.Name.Equals(topicName));
            if (existing == null)
            {
                return OperationResultExt.Fail<Topic>(CommandStatus.TopicNotFound, "Topic not found");
            }
            return OperationResultExt.Success(existing);
        }
    }
}
