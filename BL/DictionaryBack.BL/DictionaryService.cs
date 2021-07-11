using DictionaryBack.Domain;
using System;
using System.Collections.Generic;

namespace DictionaryBack.BL
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IReadRepository _readRepository;

        public DictionaryService(IReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public IEnumerable<TableRow> All()
        {
            return _readRepository.All();
        }

        public IEnumerable<TableRow> Take(int skip, int take)
        {
            return _readRepository.Take(skip, take);
        }
    }
}
