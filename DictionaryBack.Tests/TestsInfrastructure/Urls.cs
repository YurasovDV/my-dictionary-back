namespace DictionaryBack.Tests.TestsInfrastructure
{
    /// <summary>
    /// TODO: typed C# client via NSwag
    /// </summary>
    internal static class Urls
    {
        internal static class Query
        {
            internal const string GetPage = "DictionaryRead/GetPage";
            internal const string GetPageNoTracking = "DictionaryRead/GetPageNoTracking";
            internal const string GetPageDapper = "DictionaryRead/GetPageDapper";
        }

        internal static class Command
        {
            internal const string AddWord = "DictionaryCommand/";
            internal const string EditWord = "DictionaryCommand/";
            internal const string DeleteWord = "DictionaryCommand/";
        }

        internal static class Repetition
        { 
            internal const string CreateRepetitionSet = "";
            internal const string CompleteRepetition = "";
        }
    }
}
