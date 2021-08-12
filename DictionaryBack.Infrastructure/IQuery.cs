namespace DictionaryBack.Infrastructure
{
    public interface IQuery<TQueryParam, TResult>
    {
        TResult Get(TQueryParam parameters);
    }
}
