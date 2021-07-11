using DictionaryBack.Domain;
using System.Collections.Generic;

namespace DictionaryBack.BL
{
    public interface IDictionaryService
    {
        IEnumerable<TableRow> All();

        IEnumerable<TableRow> Take(int skip, int take);
    }
}
