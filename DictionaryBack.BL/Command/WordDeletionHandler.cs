using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
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
        public WordDeletionHandler(DictionaryContext dictionaryContext) : base(dictionaryContext)        {        }

        public async Task<BoolOperationResult> Delete(string term)
        {
            try
            {
                var wordExisting = await DictionaryContext.Words.FindAsync(term);
                if (wordExisting != null)
                {
                    wordExisting.IsDeleted = true;
                    DictionaryContext.SaveChanges();
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
