using DictionaryBack.DAL;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.DTOs.Command;
using DictionaryBack.Infrastructure.DTOs.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    // todo separate entity with own lifecycle
    public interface IRepetitionHandler
    {
        Task<OperationResult<WordDto[]>> CreateSet();

        Task<BoolOperationResult> CompleteRepetition(WordRepetitionResult[] words);
    }

    public class RepetitionHandler : BaseCommand, IRepetitionHandler
    {
        public RepetitionHandler(DictionaryContext dictionaryContext, ITranslationService translationService) : base(dictionaryContext, translationService)
        {
        }

        public async Task<OperationResult<WordDto[]>> CreateSet()
        {
            var set = DictionaryContext.Words
                .Where(w => w.Status == WordStatus.Learned)
                .OrderBy(w => w.LastRepetition)
                .Take(30)
                .Select(w => Mapper.Map(w))
                .ToArray();

            if (set.Any())
            {
                return OperationResultExt.Success(set);
            }

            return OperationResultExt.Fail<WordDto[]>(CommandStatus.InternalError, TranslationService.GetTranslation("No words for repetion"));
        }

        public async Task<BoolOperationResult> CompleteRepetition(WordRepetitionResult[] words)
        {
            return OperationResultExt.BoolFail(CommandStatus.InternalError, TranslationService.GetTranslation("Not implemented"));
        }
    }
}
