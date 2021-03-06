namespace DictionaryBack.Common
{
    public class DictionaryApiSettings
    {
        /// <summary>
        /// Words count to send to client
        /// </summary>
        public int MaxWordsInRequest { get; set; }

        /// <summary>
        /// Words count in one repetition set
        /// </summary>
        public int RepetitionSetSize { get; set; }

        public static readonly string SectionName = "DictionaryApiSettings";
    }
}
