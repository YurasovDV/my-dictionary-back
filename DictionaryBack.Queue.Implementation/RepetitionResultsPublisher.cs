using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.Queue;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }
    }
}
