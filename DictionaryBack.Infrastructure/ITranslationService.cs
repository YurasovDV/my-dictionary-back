namespace DictionaryBack.Infrastructure
{
    public interface ITranslationService
    {
        string GetTranslation(ErrorKey key);
    }
}
