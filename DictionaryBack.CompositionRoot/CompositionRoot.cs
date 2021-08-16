using DictionaryBack.BL.Command;
using DictionaryBack.BL.Query;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryBack.CompositionRoot
{
    public static class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DictionaryContext>(builder => 
                builder
                .UseNpgsql(configuration.GetConnectionString("WordsContext"))
                // TODO
                .EnableSensitiveDataLogging()
            );

            services.AddScoped<IDapperFacade, DapperPgFacade>();
            services.AddScoped<Seeder>();

            services.AddScoped<IAllWordsQueryHandler, AllWordsQueryHandler>();
            services.AddScoped<IWordsByTopicQueryHandler, WordsByTopicQueryHandler>();


            services.AddScoped<IWordCreationHandler, WordCreationHandler>();
            services.AddScoped<IWordEditHandler, WordEditHandler>();
            services.AddScoped<IWordDeletionHandler, WordDeletionHandler>();

        }
    }
}
