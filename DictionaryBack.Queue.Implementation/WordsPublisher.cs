using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Entities;
using DictionaryBack.Common.Queue;
using DictionaryBack.Messages;
using DictionaryBack.Queue.Implementation.Messages;
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

        public async Task PublishChangedWord(Word word)
        {
            await _publishEndpoint.Publish<WordMessage>(new WordMessageImpl()
            { 
                IsDeleted = word.IsDeleted,
                Term = word.Term
            });
        }
    }
}
