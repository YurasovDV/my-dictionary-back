using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordEditHandler
    {
        Task<OperationResult<WordDto>> Edit(WordEditModel request);
    }

    public class WordEditHandler : BaseCommand, IWordEditHandler
    {
        public WordEditHandler(DictionaryContext dictionaryContext) : base(dictionaryContext) { }

        public async Task<OperationResult<WordDto>> Edit(WordEditModel request)
        {
            // i think i hate dapper
            try
            {
                // can be rewritten for one database trip, if we receive original values from client
                var existingWord = await DictionaryContext.Words.FindAsync(request.Term);
                if (existingWord != null)
                {
                    DictionaryContext.Entry(existingWord).CurrentValues.SetValues(request);
                    await DictionaryContext.SaveChangesAsync();
                    return OperationResultExt.Success(Mapper.Map(existingWord));
                }
                return OperationResultExt.Fail<WordDto>(CommandStatus.WordNotFound, "Word does not exist");
            }
            catch (Exception ex)
            {
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, "Internal error", ex.Message);
            }
        }
    }
}
