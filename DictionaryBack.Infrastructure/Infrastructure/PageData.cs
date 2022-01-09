using System;
using System.Linq;

namespace DictionaryBack.Common
{
    public class PageData<T>
    {
        public int Total { get; set; }

        public T[] Page { get; set; }

        public static PageData<T2> From<T1, T2>(PageData<T1> from, Func<T1, T2> map) => 
            new PageData<T2>() { Page = from.Page.Select(map).ToArray(), Total = from.Total };
    }
}
