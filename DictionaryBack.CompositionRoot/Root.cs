using DictionaryBack.BL.Command;
using DictionaryBack.BL.Query;
using DictionaryBack.BL.Seeding;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using DictionaryBack.ErrorMessages;
using DictionaryBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DictionaryBack.CompositionRoot
{
    public static class Root
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            DAL(services, configuration, isDevelopment);
            BL(services);
        }

        private static void DAL(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
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

            services.AddDbContext<DictionaryContext>(action);

            services.AddScoped<IDapperFacade, DapperPgFacade>();
        }

        private static void BL(IServiceCollection services)
        {
            services.AddScoped<Seeder>();

            services.AddScoped<IAllWordsQueryHandler, AllWordsQueryHandler>();
            services.AddScoped<IWordsByTopicQueryHandler, WordsByTopicQueryHandler>();


            services.AddScoped<IWordCreationHandler, WordCreationHandler>();
            services.AddScoped<IWordEditHandler, WordEditHandler>();
            services.AddScoped<IWordDeletionHandler, WordDeletionHandler>();

            services.AddScoped<IRepetitionHandler, RepetitionHandler>();


            services.AddScoped<ITranslationService, TranslationService>();
        }
    }
}
