using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DictionaryBack.DAL
{
    public class Seeder
    {
        private readonly DictionaryContext _context;

        public Seeder(DictionaryContext context)
        {
            _context = context;
        }

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

                foreach (var row in rows)
                {
                    row.Translations = row.Translations.Distinct(new TranslationsComparer()).ToList();
                }

                _context.Words.AddRange(rows);
                _context.SaveChanges();

            }
        }
    }

    internal class TranslationsComparer : IEqualityComparer<Translation>
    {
        public bool Equals(Translation x, Translation y)
        {
            return
                string.Equals(x.Term, y.Term, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Meaning, y.Meaning, StringComparison.OrdinalIgnoreCase);

        }

        public int GetHashCode([DisallowNull] Translation obj)
        {
            return obj.Meaning.GetHashCode();
        }
    }
}
