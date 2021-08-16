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

                foreach (var tr in htmlRows)
                {
                    var tds = tr.Elements("td").ToArray();
                    var term = tds[1].Element("b").InnerText;
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
                    rows.Add(word);
                }
            }
            if (rows.Any())
            {
                var serialized = JsonSerializer.Serialize(rows,
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
