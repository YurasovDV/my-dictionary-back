using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using DictionaryBack.Common.Queue;
using DictionaryBack.Queue.Implementation;
using DictionaryBack.Common;
using DictionaryBack.Messages;

namespace DictionaryBack.Queue
{
    public static class QueueDependencyInjectionModule
    {
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            RabbitSettings opts = configuration
                .GetSection(RabbitSettings.SectionName)
                .Get<RabbitSettings>();


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

                        rabbitCfg.Publish<RepetitionEndedMessage>(x =>
                        {
                            x.ExchangeType = RabbitMQ.Client.ExchangeType.Fanout;
                        });
                    });
            });

            services.AddMassTransitHostedService();

            services.AddScoped<IRepetitionResultsPublisher, RepetitionResultsPublisher>();
            services.AddScoped<IWordsPublisher, WordsPublisher>();
        }
    }
}
