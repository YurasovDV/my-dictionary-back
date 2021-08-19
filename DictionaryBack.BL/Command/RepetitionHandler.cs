using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    // todo separate entity with own lifecycle
    public interface IRepetitionHandler
    {
        Task<OperationResult<WordDto[]>> CreateSet();

        Task<BoolOperationResult> CompleteRepetition(WordDto[] words);
    }

    public class RepetitionHandler : BaseCommand, IRepetitionHandler
    {
        public RepetitionHandler(DictionaryContext dictionaryContext, ITranslationService translationService) : base(dictionaryContext, translationService)
        {
        }

        public async Task<OperationResult<WordDto[]>> CreateSet()
        {
            return OperationResultExt.Fail<WordDto[]>(CommandStatus.InternalError, TranslationService.GetTranslation("Not implemented"));
        }

        public async Task<BoolOperationResult> CompleteRepetition(WordDto[] words)
        {
            return OperationResultExt.BoolFail(CommandStatus.InternalError, TranslationService.GetTranslation("Not implemented"));
        }
    }
}
