namespace DictionaryBack.BL.Command.Models
{
    public class WordEditModel
    {
        public string Term { get; set; }

        public string Topic { get; set; }

        public string[] Translations { get; set; }
    }
}
