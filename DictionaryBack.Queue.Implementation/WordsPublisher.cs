using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Queue;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBack.Queue.Implementation
{
    public class WordsPublisher : IWordsPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public WordsPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishChangedWord(WordDto wordDto)
        {
            await _publishEndpoint.Publish(wordDto);
        }
    }
}
