using DictionaryBack.Domain;
using System.Collections.Generic;

namespace DictionaryBack.BL
{
    public interface IDictionaryService
    {
        IEnumerable<Word> All();

        IEnumerable<Word> Take(int skip, int take);
    }
}
