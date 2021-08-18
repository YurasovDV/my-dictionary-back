using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using HtmlAgilityPack;
using DictionaryBack.Domain;

namespace DictionaryBack.DictExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rows = new List<Word>();

            using (var rd = new StreamReader($"SourceDict{Path.DirectorySeparatorChar}vocabulary_print.html"))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(await rd.ReadToEndAsync());

                var htmlRows = doc.DocumentNode
                    .SelectSingleNode("//table/tbody")
                    .Elements("tr");

                var translationsComparer = new TranslationsComparer();

                foreach (var tr in htmlRows)
                {
                    var tds = tr.Elements("td").ToArray();
                    var term = tds[1].Element("b").InnerText.Trim();
                    var translations = tds[3].InnerText
                        .Split(';', StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToArray();

                    var word = new Word
                    {
                        Term = term,
                        Translations = translations
                        .Select(t =>
                            new Translation()
                            {
                                Term = term,
                                Meaning = t
                            })
                        .ToArray()
                    };
                    word.Translations = word.Translations.Distinct(translationsComparer).ToArray();
                    rows.Add(word);
                }
            }
            if (rows.Any())
            {
                var grouped = rows.GroupBy(w => w.Term).ToList();

                List<Word> distinctRows = grouped.Select(g => g.First()).ToList();


                var serialized = JsonSerializer.Serialize(distinctRows,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin),
                    });
                File.WriteAllText("serializeddict.json", serialized);
            }
        }
    }
}
