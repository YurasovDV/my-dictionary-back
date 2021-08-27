namespace DictionaryBack.Tests.TestsInfrastructure
{
    /// <summary>
    /// TODO: typed C# client via NSwag
    /// </summary>
    internal static class Urls
    {
        internal static class Query
        {
            internal const string GetPage = "api/v1/DictionaryRead/GetPage";
            internal const string GetPageNoTracking = "api/v1/DictionaryRead/GetPageNoTracking";
            internal const string GetPageDapper = "api/v1/DictionaryRead/GetPageDapper";
        }

        internal static class Command
        {
            internal const string AddWord = "api/v1/DictionaryCommand/";
            internal const string EditWord = "api/v1/DictionaryCommand/";
            internal const string DeleteWord = "api/v1/DictionaryCommand/";
        }

        internal static class Repetition
        { 
            internal const string CreateRepetitionSet = "api/v1/Repetition/CreateRepetitionSet";
            internal const string CompleteRepetition = "api/v1/Repetition/CompleteRepetition";
        }
    }
}
