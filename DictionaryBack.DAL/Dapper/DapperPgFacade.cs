using Dapper;
using DictionaryBack.Domain;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.DAL.Dapper
{
    public class DapperPgFacade : IDapperFacade
    {
        private readonly IConfiguration _configuration;

        private static class PostgresqlText
        {
            public static readonly string GetAll = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        ORDER BY w.term, t.term, t.meaning";


            public static readonly string GetPage = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        ORDER BY w.term, t.term, t.meaning
                                                        offset @Skip limit @Take";

            public static readonly string GetPageWithTopic = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        where w.topic ~ @Topic
                                                        ORDER BY w.term, t.term, t.meaning
                                                        offset @Skip limit @Take";


        }

        public DapperPgFacade(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Word>> GetAll()
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("WordsContext"));
            List<Word> words = (await conn.QueryAsync<Word, Translation, Word>(PostgresqlText.GetAll,
                (w, t) =>
                {
                    if (w.Translations == null)
                    {
                        w.Translations = new List<Translation>();
                    }
                    w.Translations.Add(t);
                    return w;
                },
                splitOn: "term")).ToList();
            return words;
        }

        public async Task<IEnumerable<Word>> GetPage(int take, int skip)
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("WordsContext"));
            var parameters = new DynamicParameters(new { Skip = skip, Take = take });

            List<Word> words = (await conn.QueryAsync<Word, Translation, Word>(PostgresqlText.GetPage,
                (w, t) =>
                {
                    if (w.Translations == null)
                    {
                        w.Translations = new List<Translation>();
                    }
                    w.Translations.Add(t);
                    return w;
                },
                parameters,
                splitOn: "term"))
                .ToList();

            return words;
        }
    }
}
