using DictionaryBack.Domain;
using System.Linq;

namespace DictionaryBack.Infrastructure
{
    public interface IQueryRepository
    {
        IQueryable<Word> Words { get; }
    }
}
