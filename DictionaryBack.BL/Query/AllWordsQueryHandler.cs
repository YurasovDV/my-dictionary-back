using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using DictionaryBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    /// <summary>
    /// Only for perf test
    /// </summary>
    public interface IAllWordsQueryHandler
    {

        Task<OperationResult<IEnumerable<WordDto>>> GetWordsNoTrackingAsync();

        Task<OperationResult<IEnumerable<WordDto>>> GetWithDapper();
    }


    public class AllWordsQueryHandler : IAllWordsQueryHandler
    {
        private readonly DictionaryContext _dictionaryContext;
        private readonly IDapperFacade _dapperFacade;

        public AllWordsQueryHandler(DictionaryContext dictionaryContext, IDapperFacade dapperFacade)
        {
            _dictionaryContext = dictionaryContext;
            _dapperFacade = dapperFacade;
        }

        public async Task<OperationResult<IEnumerable<WordDto>>> GetWordsNoTrackingAsync()
        {
            IEnumerable<WordDto> data = (await _dictionaryContext.Words.AsNoTracking().ToListAsync())
                .Select(Mapper.Map)
                .ToList();
            return OperationResultExt.Success(data);
        }

        public async Task<OperationResult<IEnumerable<WordDto>>> GetWithDapper()
        {
            IEnumerable<WordDto> data = (await _dapperFacade.GetAll())
                .Select(Mapper.Map)
                .ToList();

            return OperationResultExt.Success(data);
        }
    }
}
