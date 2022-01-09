using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Enums;
using DictionaryBack.Common.Localization;
using DictionaryBack.Common.Queue;
using DictionaryBack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    // todo separate entity with own lifecycle?
    public interface IRepetitionHandler
    {
        Task<OperationResult<WordDto[]>> CreateSet();

        Task<BoolOperationResult> CompleteRepetition(WordRepetitionResult[] words);
    }

    public class RepetitionHandler : BaseCommand, IRepetitionHandler
    {
        private readonly DictionaryApiSettings _settings;
        private readonly IRepetitionResultsPublisher _repetitionResultsPublisher;
        private readonly ILogger<RepetitionHandler> _logger;

        public RepetitionHandler(DictionaryContext dictionaryContext, 
            ITranslationService translationService,
            IRepetitionResultsPublisher repetitionResultsPublisher,
            IOptions<DictionaryApiSettings> options,
            ILogger<RepetitionHandler> logger) : base(dictionaryContext, translationService, null /* TODO: fix hierarchy */)
        {
            _settings = options.Value;
            _repetitionResultsPublisher = repetitionResultsPublisher;
            _logger = logger;
        }

        public async Task<OperationResult<WordDto[]>> CreateSet()
        {
            var set = await DictionaryContext.Words
                .Where(w => w.Status == WordStatus.Learned)
                .OrderBy(w => w.LastRepetition)
                .Take(_settings.RepetitionSetSize)
                .Select(w => Mapper.Map(w))
                .ToArrayAsync();

            if (set.Any())
            {
                return OperationResultExt.Success(set);
            }

            return OperationResultExt.Fail<WordDto[]>(CommandStatus.InternalError, TranslationService.GetTranslation(ErrorKey.NoWordsForRepetion));
        }

        public async Task<BoolOperationResult> CompleteRepetition(WordRepetitionResult[] wordsRepetitionResults)
        {
            try
            {
                var termsUsed = wordsRepetitionResults
                    .Select(w => w.Term)
                    .ToArray();

                var wordsToUpdate = await DictionaryContext.Words
                    .Where(w => termsUsed.Contains(w.Term))
                    .ToArrayAsync();

                var joined = wordsToUpdate
                    .Join(wordsRepetitionResults,
                            wordFromDb => wordFromDb.Term,
                            wordWithResult => wordWithResult.Term,
                            (wordFromDb, wordWithResult) => 
                                new 
                                { 
                                    Word = wordFromDb, 
                                    RepetitionStatus = wordWithResult.RepetitionStatus 
                                })
                    .ToArray();

                foreach (var pair in joined)
                {
                    pair.Word.LastRepetition = DateTime.UtcNow;
                    pair.Word.RepetitionStatus = pair.RepetitionStatus;
                }

                await DictionaryContext.SaveChangesAsync();

                await _repetitionResultsPublisher.PublishResult(wordsRepetitionResults);

                return OperationResultExt.BoolSuccess();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return OperationResultExt.BoolFail(CommandStatus.InternalError, TranslationService.GetTranslation(ErrorKey.InternalError), ex.Message);
            }
        }
    }
}
