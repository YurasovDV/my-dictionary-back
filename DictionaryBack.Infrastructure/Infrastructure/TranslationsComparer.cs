using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DictionaryBack.Common.Entities;

namespace DictionaryBack.Common
{
    public class TranslationsComparer : IEqualityComparer<Translation>
    {
        public bool Equals(Translation x, Translation y)
        {
            return
                string.Equals(x.Term, y.Term, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Meaning, y.Meaning, StringComparison.OrdinalIgnoreCase);

        }

        public int GetHashCode([DisallowNull] Translation obj)
        {
            return obj.Meaning.GetHashCode();
        }
    }
}
