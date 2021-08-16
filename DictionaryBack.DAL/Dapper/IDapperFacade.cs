using DictionaryBack.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.DAL.Dapper
{
    public interface IDapperFacade
    {
        Task<IEnumerable<Word>> GetAll();
    }
}
