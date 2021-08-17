using DictionaryBack.BL.Command;
using DictionaryBack.BL.Query;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DictionaryBack.CompositionRoot
{
    public static class CompositionRoot
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            Action<DbContextOptionsBuilder> action;

            if (isDevelopment)
            {
                action = builder =>
                            builder
                            .UseNpgsql(configuration.GetConnectionString("WordsContext"))
                            .EnableSensitiveDataLogging()
                            .LogTo(Console.WriteLine);
            }
            else
            {
                action = builder =>
                            builder
                            .UseNpgsql(configuration.GetConnectionString("WordsContext"));
            }

            services.AddDbContextPool<DictionaryContext>(action);

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
