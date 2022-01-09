using DictionaryBack.Common;
using DictionaryBack.Common.Entities;
using DictionaryBack.Common.Localization;
using DictionaryBack.DAL;
using System;
using System.Linq;

namespace DictionaryBack.BL.Command
{
    public class TopicFinder
    {
        private readonly ITranslationService _translationService;

        protected DictionaryContext DictionaryContext { get; private set; }

        public TopicFinder(DictionaryContext dictionaryContext, ITranslationService translationService)
        {
            DictionaryContext = dictionaryContext ?? throw new ArgumentNullException(nameof(dictionaryContext));
            this._translationService = translationService;
        }

        internal OperationResult<Topic> FindTopic(string topicName)
        {
            var existing = DictionaryContext.Topics.FirstOrDefault(t => t.Name.Equals(topicName));
            if (existing == null)
            {
                return OperationResultExt.Fail<Topic>(CommandStatus.TopicNotFound, _translationService.GetTranslation(ErrorKey.TopicNotFound));
            }
            return OperationResultExt.Success(existing);
        }
    }
}
