using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Entities;
using DictionaryBack.Common.Localization;
using DictionaryBack.Common.Queue;
using DictionaryBack.DAL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordEditHandler
    {
        Task<OperationResult<WordDto>> Edit(WordEditModel request);
    }

    public class WordEditHandler : BaseCommand, IWordEditHandler
    {
        private readonly ILogger<WordEditHandler> _logger;

        public WordEditHandler(DictionaryContext dictionaryContext, ITranslationService translationService, ILogger<WordEditHandler> logger, IWordsPublisher wordsPublisher) : base(dictionaryContext, translationService, wordsPublisher)
        {
            this._logger = logger;
        }

        public async Task<OperationResult<WordDto>> Edit(WordEditModel request)
        {
            // i think i hate dapper
            try
            {
                // can be rewritten for one database trip, if we receive original values from client
                var existingWord = await DictionaryContext.Words.FindAsync(request.Term);
                if (existingWord != null)
                {
                    // for plain properties: DictionaryContext.Entry(existingWord).CurrentValues.SetValues(request);

                    var editResult = EditInternal(existingWord, request);
                    if (editResult.IsSuccessful())
                    {
                        await DictionaryContext.SaveChangesAsync();
                        await WordsPublisher.PublishChangedWord(existingWord);
                        return OperationResultExt.Success(Mapper.Map(existingWord));
                    }
                    return editResult;
                }
                return OperationResultExt.Fail<WordDto>(CommandStatus.WordNotFound, TranslationService.GetTranslation(ErrorKey.WordDoesNotExist));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, TranslationService.GetTranslation(ErrorKey.InternalError), ex.Message);
            }
        }

        private OperationResult<WordDto> EditInternal(Word existingWord, WordEditModel request)
        {
            if (request.Topic != existingWord?.Topic.Name)
            {
                var found = TopicFinder.FindTopic(request.Topic);
                if (!found.IsSuccessful())
                {
                    return OperationResultExt.Fail<WordDto>(found.StatusCode, found.ErrorText);
                }
                existingWord.Topic = found.Data;
            }

            if (existingWord.Status != request.Status)
            {
                existingWord.Status = request.Status;
            }

            existingWord.Translations = GetUpdatedTranslationsList(existingWord, request);

            return OperationResultExt.Success(Mapper.Map(existingWord));
        }

        private static Translation[] GetUpdatedTranslationsList(Word existingWord, WordEditModel request)
        {
            Dictionary<string, Translation> result = existingWord.Translations.ToDictionary(t => t.Meaning);

            // maybe plain array is good enough
            var existingTranslations = result.Keys.ToHashSet();

            var translationsAdded = request.Translations.Except(existingTranslations).ToArray();
            var translationsRemoved = existingTranslations.Except(request.Translations).ToArray();

            foreach (var added in translationsAdded)
            {
                result[added] = new Translation()
                {
                    Meaning = added,
                };
            }

            foreach (var removed in translationsRemoved)
            {
                result.Remove(removed);
            }

            return result.Values.ToArray();
        }
    }
}
