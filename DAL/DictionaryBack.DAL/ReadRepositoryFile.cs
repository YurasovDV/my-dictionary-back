using DictionaryBack.Domain;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DictionaryBack.DAL
{
    public class ReadRepositoryFile : IReadRepository
    {
        private AsyncLazy<TableRow[]> TableRowsLazy = new(
           async () => 
            {
                using var stream = File.OpenRead(@"Static\serializeddict.json");
                TableRow[] rows = await JsonSerializer.DeserializeAsync<TableRow[]>(stream, 
                    new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin),
                    });

                return rows;
            });


        public ReadRepositoryFile()
        {
            TableRowsLazy.Start();
        }

        public IEnumerable<TableRow> All()
        {
            return TableRowsLazy.Task.Result;
        }

        public IEnumerable<TableRow> Take(int skip, int take)
        {
            return TableRowsLazy.Task.Result.Skip(skip).Take(take).ToArray();
        }
    }
}
