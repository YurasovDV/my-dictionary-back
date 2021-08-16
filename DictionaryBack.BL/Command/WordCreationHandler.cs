using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordCreationHandler
    {
        Task<OperationResult<WordDto>> Create(WordCreationModel request);
    }

    public class WordCreationHandler : BaseCommand, IWordCreationHandler
    {
        public WordCreationHandler(DictionaryContext dictionaryContext) : base(dictionaryContext) { }

        public async Task<OperationResult<WordDto>> Create(WordCreationModel request)
        {
            try
            {
                var word = Mapper.Map(request);
                DictionaryContext.Words.Add(word);
                await DictionaryContext.SaveChangesAsync();
                return OperationResultExt.Success(Mapper.Map(word));
            }
            catch (Exception)
            {
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, "Internal error");
            }
        }
    }
}
