using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DictionaryBack.DAL
{
    public class Seeder
    {
        private readonly DictionaryContext _context;

        public Seeder(DictionaryContext context)
        {
            _context = context;
        }

        // stream?
        public void MigrateAndSeed(string dictionaryJson)
        {
            _context.Database.Migrate();

            if (!_context.Words.Any())
            {
                Word[] rows = JsonSerializer.Deserialize<Word[]>(dictionaryJson,
                    new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin),
                    });

                _context.Words.AddRange(rows);
                _context.SaveChanges();

            }
        }
    }

}
