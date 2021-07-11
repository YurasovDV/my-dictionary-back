using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBack.Domain
{
    public interface IReadRepository
    {
        IEnumerable<TableRow> All();

        IEnumerable<TableRow> Take(int skip, int take);
    }
}
