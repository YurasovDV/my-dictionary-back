using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Localization;
using DictionaryBack.Common.Queue;
using DictionaryBack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordCreationHandler
    {
        Task<OperationResult<WordDto>> Create(WordCreationModel request);
    }

    public class WordCreationHandler : BaseCommand, IWordCreationHandler
    {
        private readonly ILogger<WordCreationHandler> _logger;

        public WordCreationHandler(DictionaryContext dictionaryContext, ITranslationService translationService, ILogger<WordCreationHandler> logger, IWordsPublisher wordsPublisher) : base(dictionaryContext, translationService, wordsPublisher)
        {
            _logger = logger;
        }

        public async Task<OperationResult<WordDto>> Create(WordCreationModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Term))
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, TranslationService.GetTranslation(ErrorKey.NoWordProvided));
                }

                if (request.Translations == null || request.Translations.Length == 0 || request.Translations.Any(r => string.IsNullOrWhiteSpace(r)))
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, TranslationService.GetTranslation(ErrorKey.InvalidTranslations));
                }

                var word = Mapper.Map(request);

                var existingTopic = TopicFinder.FindTopic(word.Topic.Name);
                if (existingTopic.IsSuccessful())
                {
                    word.TopicId = existingTopic.Data.Id;
                    word.Topic = existingTopic.Data;
                }
                else
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, TranslationService.GetTranslation(ErrorKey.TopicNotFound));
                }

                var existingWord = DictionaryContext.Words.IgnoreQueryFilters().FirstOrDefault(row => row.Term == word.Term);

                if (existingWord != null)
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, TranslationService.GetTranslation(ErrorKey.WordAlreadyExists));
                }

                DictionaryContext.Words.Add(word);
                await DictionaryContext.SaveChangesAsync();
                await WordsPublisher.PublishChangedWord(word);
                return OperationResultExt.Success(Mapper.Map(word));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, TranslationService.GetTranslation(ErrorKey.InternalError));
            }
        }
    }
}
