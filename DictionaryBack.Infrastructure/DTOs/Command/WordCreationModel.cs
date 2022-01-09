namespace DictionaryBack.Common.DTOs.Command
{
    public class WordCreationModel
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translations { get; set; }
    }
}
