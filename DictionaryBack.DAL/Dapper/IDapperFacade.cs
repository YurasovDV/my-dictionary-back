using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.DAL.Dapper
{
    public interface IDapperFacade
    {
        Task<IEnumerable<Word>> GetAllAsync();

        Task<PageData<Word>> GetPage(WordsByTopicRequest request);
    }
}
