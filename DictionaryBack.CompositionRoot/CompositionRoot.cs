using DictionaryBack.BL;
using DictionaryBack.BL.Query;
using DictionaryBack.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DictionaryBack.CompositionRoot
{
    public static class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DictionaryContext>(builder => builder.UseNpgsql(configuration.GetConnectionString("WordsContext")));
            services.AddScoped<Seeder>();

            services.AddScoped<IAllWordsQueryHandler, AllWordsQueryHandler>();
            services.AddScoped<BL.Query.IWordsByTopicQueryHandler, WordsByTopicQueryHandler>();
        }
    }
}
