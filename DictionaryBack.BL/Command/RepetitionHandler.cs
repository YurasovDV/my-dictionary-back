using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IRepetitionHandler
    {
        Task<OperationResult<WordDto[]>> CreateSet();
    }

    public class RepetitionHandler : BaseCommand, IRepetitionHandler
    {
        public RepetitionHandler(DictionaryContext dictionaryContext) : base(dictionaryContext)
        {
        }

        public async Task<OperationResult<WordDto[]>> CreateSet()
        {
            return OperationResultExt.Fail<WordDto[]>(CommandStatus.InternalError, "Not implemented");
        }
    }


}
