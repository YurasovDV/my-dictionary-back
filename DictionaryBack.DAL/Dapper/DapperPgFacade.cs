using Dapper;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure.Requests;
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


            /// <summary>
            /// only limit offset are applied
            /// </summary>
            public static readonly string GetPageNoTextSearch = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        ORDER BY w.term, t.term, t.meaning
                                                        offset @Skip limit @Take";

            /// <summary>
            /// using only topic
            /// </summary>
            public static readonly string GetPageWithTopic = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        WHERE w.topic LIKE @Topic
                                                        ORDER BY w.term, t.term, t.meaning
                                                        OFFSET @Skip LIMIT @Take";

            /// <summary>
            /// using both term and topic
            /// </summary>
            public static readonly string GetPageWithTopicAndQuery = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        WHERE w.topic LIKE @Topic and w.term LIKE @SearchTerm
                                                        ORDER BY w.term, t.term, t.meaning
                                                        offset @Skip limit @Take";

            /// <summary>
            /// ignore topic, only term value
            /// </summary>
            public static readonly string GetPageWithQuery = @"SELECT w.term, w.is_deleted, w.topic, t.term, t.meaning, t.is_deleted
                                                        FROM words AS w
                                                        LEFT JOIN translations AS t ON w.term = t.term
                                                        WHERE w.term LIKE @SearchTerm
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

        public async Task<IEnumerable<Word>> GetPage(WordsByTopicRequest request)
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("WordsContext"));
            // bad practice
            var parameters = new DynamicParameters(new { request.Skip, request.Take, SearchTerm = $"%{request.SearchTerm}%", request.Topic });
            var query = SelectQuery(request);
            List<Word> words = (await conn.QueryAsync<Word, Translation, Word>(query,
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

        private string SelectQuery(WordsByTopicRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchTerm) && !string.IsNullOrEmpty(request.Topic)) 
            {
                return PostgresqlText.GetPageWithTopicAndQuery;
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                return PostgresqlText.GetPageWithQuery;
            }

            if (!string.IsNullOrEmpty(request.Topic))
            {
                return PostgresqlText.GetPageWithTopic;
            }

            return PostgresqlText.GetPageNoTextSearch;
        }
    }
}
