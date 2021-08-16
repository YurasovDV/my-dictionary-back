using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    public interface IWordsByTopicQueryHandler
    {
        Task<List<WordDto>> GetWordsAsync(WordsByTopicRequest request);
    }

    public class WordsByTopicQueryHandler : IWordsByTopicQueryHandler
    {
        private readonly DictionaryContext _dictionaryContext;

        public WordsByTopicQueryHandler(DictionaryContext dictionaryContext)
        {
            _dictionaryContext = dictionaryContext;
        }

        public async Task<List<WordDto>> GetWordsAsync(WordsByTopicRequest request)
        {
            var query = _dictionaryContext.Words.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Topic))
            {
                query = query.Where(w => w.Topic.Contains(request.Topic));
            }

            if (request.Skip != null || request.Take != null)
            {
                query = query.OrderBy(w => w.Term);
                if (request.Skip != null)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Take != null && request.Take > 0)
                {
                    query = query.Take(request.Take.Value);
                }
            }

            return await query.Select(w => Mapper.Map(w)).ToListAsync();
        }
    }
}
