using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using DictionaryBack.Common.Queue;
using DictionaryBack.Queue.Implementation;

namespace DictionaryBack.Queue
{
    public static class QueueDependencyInjectionModule
    {
        public static void Init(IServiceCollection services, IConfiguration configuration, Common.RabbitSettings opts)
        {
            services.AddMassTransit(busCfg =>
            {
                busCfg
                .UsingRabbitMq(
                    (context, rabbitCfg) => 
                    {
                        rabbitCfg.Host(opts.Host, h =>
                        {
                            h.Username(opts.Username);
                            h.Password(opts.Password);
                        });
                    });
            });

            services.AddMassTransitHostedService();

            services.AddScoped<IRepetitionResultsPublisher, RepetitionResultsPublisher>();
            services.AddScoped<IWordsPublisher, WordsPublisher>();
        }
    }
}
