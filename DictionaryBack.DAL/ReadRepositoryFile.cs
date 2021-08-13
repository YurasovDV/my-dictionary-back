using DictionaryBack.Domain;
using DictionaryBack.Infrastructure;
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
    public class ReadRepositoryFile : IQueryRepository
    {
        private AsyncLazy<Word[]> TableRowsLazy = new(
           async () => 
            {
                using var stream = File.OpenRead($"Static{Path.DirectorySeparatorChar}serializeddict.json");
                Word[] rows = await JsonSerializer.DeserializeAsync<Word[]>(stream, 
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

        public IEnumerable<Word> All()
        {
            return TableRowsLazy.Task.Result;
        }

        public IEnumerable<Word> Take(int skip, int take)
        {
            return TableRowsLazy.Task.Result.Skip(skip).Take(take).ToArray();
        }

        public IQueryable<Word> Words => All().AsQueryable();
    }
}
