namespace DictionaryBack.Common.Localization
{
    public interface ITranslationService
    {
        string GetTranslation(ErrorKey key);
    }
}
