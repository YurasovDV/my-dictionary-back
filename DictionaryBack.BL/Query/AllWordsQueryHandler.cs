using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    public interface IAllWordsQueryHandler
    {
        Task<List<WordDto>> GetWordsAsync();

        Task<IEnumerable<WordDto>> GetWordsNoTrackingAsync();

        Task<IEnumerable<WordDto>> GetWithDapper();
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

        public async Task<List<WordDto>> GetWordsAsync()
        {
            return await _dictionaryContext.Words.Select(w => Mapper.Map(w)).ToListAsync();
        }

        public async Task<IEnumerable<WordDto>> GetWordsNoTrackingAsync()
        {
            return await _dictionaryContext.Words.AsNoTracking().Select(w => Mapper.Map(w)).ToListAsync();
        }

        public async Task<IEnumerable<WordDto>> GetWithDapper()
        {
            return (await _dapperFacade.GetAll()).Select(w => Mapper.Map(w)).ToList();
        }
    }
}
