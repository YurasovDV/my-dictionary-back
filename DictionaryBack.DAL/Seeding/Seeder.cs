using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
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


                var defaultTopic = new Topic()
                {
                    IsDeleted = false,
                    Name = "Default",
                };

                var existing = _context.Topics.FirstOrDefault(t => t.Name == defaultTopic.Name);
                if (existing == null)
                {
                    _context.Topics.Add(defaultTopic);
                    _context.SaveChanges();
                }
                else
                {
                    defaultTopic = existing;
                }

                var comparer = new TranslationsComparer();

                // entirely no reason to use Partitioner
                var partitioner = Partitioner.Create(rows);
                var partitions = partitioner.GetPartitions(rows.Count() / 200);

                foreach (var partition in partitions)
                {
                    while (partition.MoveNext())
                    {
                        var word = partition.Current;
                        word.Translations = word.Translations.Distinct(comparer).ToList();
                        word.Topic = defaultTopic;
                        _context.Words.Add(word);
                    }
                    _context.SaveChanges();
                }
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
