using DictionaryBack.Domain;
using DictionaryBack.Infrastructure.DTOs.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.DAL.Dapper
{
    public interface IDapperFacade
    {
        Task<IEnumerable<Word>> GetAllAsync();

        Task<IEnumerable<Word>> GetPage(WordsByTopicRequest request);
    }
}
