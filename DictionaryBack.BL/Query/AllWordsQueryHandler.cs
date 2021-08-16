using DictionaryBack.DAL;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    public interface IAllWordsQueryHandler
    {
        Task<List<Word>> GetWordsAsync();
    }


    public class AllWordsQueryHandler : IAllWordsQueryHandler
    {
        private readonly DictionaryContext _dictionaryContext;

        public AllWordsQueryHandler(DictionaryContext dictionaryContext)
        {
            _dictionaryContext = dictionaryContext;
        }

        public async Task<List<Word>> GetWordsAsync()
        {
            return await _dictionaryContext.Words.ToListAsync();
        }
    }
}
