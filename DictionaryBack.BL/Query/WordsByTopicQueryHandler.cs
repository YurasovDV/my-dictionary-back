using DictionaryBack.DAL;
using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    public interface IWordsByTopicQueryHandler
    {
        Task<List<Word>> GetWordsAsync(WordsByTopicRequest request);
    }


    public class WordsByTopicQueryHandler : IWordsByTopicQueryHandler
    {
        private readonly DictionaryContext _dictionaryContext;

        public WordsByTopicQueryHandler(DictionaryContext dictionaryContext)
        {
            _dictionaryContext = dictionaryContext;
        }

        public async Task<List<Word>> GetWordsAsync(WordsByTopicRequest request)
        {
            var query = _dictionaryContext.Words.Where(w => w.Topic.Contains(request.Topic));

            if (request.Skip != null || request.Take != null)
            {
                query = query.OrderBy(w => w.Term);
                if (request.Skip != null)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Take != null)
                {
                    query = query.Take(request.Take.Value);
                }
            }

            return await query.ToListAsync();
        }
    }
}
