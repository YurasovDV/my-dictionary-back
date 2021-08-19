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
            public static readonly string GetAll = 
                @"SELECT t.term, t.is_deleted, t.last_repetition, t.status, t.topic_id, t0.id, t0.is_deleted, t0.name, t1.term, t1.meaning, t1.is_deleted
                   FROM (
                       SELECT w.term, w.is_deleted, w.last_repetition, w.status, w.topic_id
                       FROM words AS w
                       ORDER BY w.term
                   ) AS t
                   INNER JOIN topics AS t0 ON t.topic_id = t0.id
                   LEFT JOIN translations AS t1 ON t.term = t1.term
                   ORDER BY t.term, t0.id, t1.term, t1.meaning";


            /// <summary>
            /// only limit offset are applied
            /// </summary>
            public static readonly string GetPageNoSearch =
                @"SELECT t.term, t.is_deleted, t.last_repetition, t.status, t.topic_id, t0.id, t0.is_deleted, t0.name, t1.term, t1.meaning, t1.is_deleted
                    FROM (
                        SELECT w.term, w.is_deleted, w.last_repetition, w.status, w.topic_id
                        FROM words AS w
                        ORDER BY w.term
                    LIMIT @Take OFFSET @Skip
                    ) AS t
                    INNER JOIN topics AS t0 ON t.topic_id = t0.id
                    LEFT JOIN translations AS t1 ON t.term = t1.term
                    ORDER BY t.term, t0.id, t1.term, t1.meaning;";

            /// <summary>
            /// ignore topic, only term value
            /// </summary>
            public static readonly string GetPageWithQuery = 
                @"SELECT t.term, t.is_deleted, t.last_repetition, t.status, t.topic_id, t0.id, t0.is_deleted, t0.name, t1.term, t1.meaning, t1.is_deleted
                     FROM (
                         SELECT w.term, w.is_deleted, w.last_repetition, w.status, w.topic_id
                         FROM words AS w
                         WHERE (strpos(w.term, @SearchTerm) > 0)
                         ORDER BY w.term
                         LIMIT @Take OFFSET @Skip
                     ) AS t
                     INNER JOIN topics AS t0 ON t.topic_id = t0.id
                     LEFT JOIN translations AS t1 ON t.term = t1.term
                     ORDER BY t.term, t0.id, t1.term, t1.meaning";


            /// <summary>
            /// using only topic
            /// </summary>
            public static readonly string GetPageWithTopic =
                @"SELECT t0.term, t0.is_deleted, t0.last_repetition, t0.status, t0.topic_id, t0.id, t0.is_deleted0, t0.name, t1.term, t1.meaning, t1.is_deleted
                    FROM (
                        SELECT w.term, w.is_deleted, w.last_repetition, w.status, w.topic_id, t.id, t.is_deleted AS is_deleted0, t.name
                        FROM words AS w
                        INNER JOIN topics AS t ON w.topic_id = t.id
                        WHERE (strpos(t.name, @Topic::citext) > 0)
                        ORDER BY w.term
                        LIMIT @Take OFFSET @Skip
                    ) AS t0
                    LEFT JOIN translations AS t1 ON t0.term = t1.term
                    ORDER BY t0.term, t0.id, t1.term, t1.meaning";


            /// <summary>
            /// using both term and topic
            /// </summary>
            public static readonly string GetPageWithTopicAndQuery =

                @"SELECT t0.term, t0.is_deleted, t0.last_repetition, t0.status, t0.topic_id, t0.id, t0.is_deleted0, t0.name, t1.term, t1.meaning, t1.is_deleted
                        FROM (
                            SELECT w.term, w.is_deleted, w.last_repetition, w.status, w.topic_id, t.id, t.is_deleted AS is_deleted0, t.name
                            FROM words AS w
                            INNER JOIN topics AS t ON w.topic_id = t.id
                            WHERE (strpos(t.name, @Topic::citext) > 0) AND (strpos(w.term, @SearchTerm) > 0)
                            ORDER BY w.term
                            LIMIT @Take OFFSET @Skip
                        ) AS t0
                        LEFT JOIN translations AS t1 ON t0.term = t1.term
                        ORDER BY t0.term, t0.id, t1.term, t1.meaning";
        }

        public DapperPgFacade(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Word>> GetAllAsync()
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
            var deduplicatedValues = new Dictionary<string, Word>();

            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("WordsContext"));
            var parameters = new { request.Skip, request.Take, request.Topic, request.SearchTerm };
            var query = SelectQuery(request);
            List<Word> words = (await conn.QueryAsync<Word, Topic, Translation, Word>(query,
                (word, topic, translation) =>
                {
                    if (deduplicatedValues.TryGetValue(word.Term, out var existing))
                    {
                        existing.Translations.Add(translation);
                        return existing;
                    }
                    else
                    {
                        if (word.Translations == null)
                        {
                            word.Translations = new List<Translation>();
                        }
                        word.Translations.Add(translation);
                        word.Topic = topic;
                        word.TopicId = topic.Id;
                        deduplicatedValues[word.Term] = word;
                        return word;
                    }
                },
                parameters,
                splitOn: "id,term"))
                .ToList();

            return deduplicatedValues.Values;
        }

        private static string SelectQuery(WordsByTopicRequest request)
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

            return PostgresqlText.GetPageNoSearch;
        }
    }
}
