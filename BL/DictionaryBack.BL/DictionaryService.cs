using DictionaryBack.Domain;
using System;
using System.Collections.Generic;

namespace DictionaryBack.BL
{
    public class DictionaryService : IDictionaryService
    {
        public DictionaryService()
        {
        }

        public IEnumerable<Word> All()
        {
            throw new Exception();
        }

        public IEnumerable<Word> Take(int skip, int take)
        {
            throw new Exception();
        }
    }
}
