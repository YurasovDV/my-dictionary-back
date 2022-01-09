using System;
using System.Linq;

namespace DictionaryBack.Common
{
    public static class PageDataExt
    {
        public static PageData<TRes> From<T, TRes>(PageData<T> from, Func<T, TRes> map) => 
            new PageData<TRes>() 
            { 
                Page = from.Page.Select(map).ToArray(), 
                Total = from.Total 
            };
    }
}
