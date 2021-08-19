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

        public void Migrate()
        {
            _context.Database.Migrate();
        }

        public void Seed(string dictionaryJson)
        {
            _context.Database.OpenConnection();
            ((Npgsql.NpgsqlConnection)_context.Database.GetDbConnection()).ReloadTypes();


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
                    Name = Constants.DefaultTopic,
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
                var partitions = partitioner.GetPartitions(rows.Length / 200);

                foreach (var partition in partitions)
                {
                    while (partition.MoveNext())
                    {
                        var word = partition.Current;
                        word.Topic = defaultTopic;
                        _context.Words.Add(word);
                    }
                    _context.SaveChanges();
                }
            }
        }

        public void DropDatabase()
        {
            _context.Database.EnsureDeleted();

            //_context.Database.EnsureCreated();
        }
    }
}
