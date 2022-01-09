using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.Queue;
using DictionaryBack.Messages;
using DictionaryBack.Queue.Implementation.Messages;
using MassTransit;
using System.Threading.Tasks;

namespace DictionaryBack.Queue.Implementation
{
    public class RepetitionResultsPublisher : IRepetitionResultsPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RepetitionResultsPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishResult(WordRepetitionResult[] wordsRepetitionResults)
        {
            await _publishEndpoint.Publish<RepetitionEndedMessage>(new RepetitionEndedMessageImpl()
            { 
                WordsRepetitionResults = wordsRepetitionResults,
            });
        }
    }
}
