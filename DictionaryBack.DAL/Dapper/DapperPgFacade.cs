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
        private string _connString;

        private static class PostgresqlText
        { 
            public static readonly string GetAll = "SELECT term, topic, translations, is_deleted FROM public.words;";
        }

        public DapperPgFacade(IConfiguration configuration)
        {
           _connString = configuration.GetConnectionString("WordsContext");
        }

        public async Task<IEnumerable<Word>> GetAll()
        {
            using (var conn = new NpgsqlConnection(_connString))
            {
                await conn.OpenAsync();

                var words = (await conn.QueryAsync<Word>(PostgresqlText.GetAll)).ToList();

                return words;
            }
        }
    }
}
