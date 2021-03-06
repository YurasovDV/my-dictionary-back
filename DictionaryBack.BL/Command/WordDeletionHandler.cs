using DictionaryBack.Common;
using DictionaryBack.Common.Localization;
using DictionaryBack.Common.Queue;
using DictionaryBack.DAL;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordDeletionHandler
    {
        Task<BoolOperationResult> Delete(string term);
    }

    public class WordDeletionHandler : BaseCommand, IWordDeletionHandler
    {
        public WordDeletionHandler(DictionaryContext dictionaryContext, ITranslationService translationService, IWordsPublisher wordsPublisher) : base(dictionaryContext, translationService, wordsPublisher) { }

        public async Task<BoolOperationResult> Delete(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return OperationResultExt.BoolFail(CommandStatus.InvalidRequest, "No word provided");
                }

                var wordExisting = await DictionaryContext.Words.FindAsync(term);
                if (wordExisting != null)
                {
                    wordExisting.IsDeleted = true;
                    DictionaryContext.SaveChanges();
                    await WordsPublisher.PublishChangedWord(wordExisting);
                    return OperationResultExt.BoolSuccess();
                }

                return OperationResultExt.BoolFail(CommandStatus.WordNotFound, "Word does not exist");
            }
            catch (Exception)
            {
                return OperationResultExt.BoolFail(CommandStatus.InternalError, "Internal error");
            }
        }
    }
}
