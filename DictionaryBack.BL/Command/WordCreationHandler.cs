using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.DTOs.Command;
using DictionaryBack.Infrastructure.DTOs.Query;
using Microsoft.EntityFrameworkCore;
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
        public WordCreationHandler(DictionaryContext dictionaryContext, ITranslationService translationService) : base(dictionaryContext, translationService) { }

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
                return OperationResultExt.Success(Mapper.Map(word));
            }
            catch (Exception)
            {
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, TranslationService.GetTranslation(ErrorKey.InternalError));
            }
        }
    }
}
