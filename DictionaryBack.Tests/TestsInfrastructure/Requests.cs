﻿using DictionaryBack.BL.Command.Models;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure.Requests;
using System;

namespace DictionaryBack.Tests.TestsInfrastructure
{
    internal static class Requests
    {
        internal static class Query
        {
            internal static WordsByTopicRequest GetRequestForFirstKWords(int take = 20) => new WordsByTopicRequest()
            {
                Skip = 0,
                Take = take,
                SearchTerm = null,
                Topic = null,
            };

            internal static WordsByTopicRequest GetRequestForFirst20WordsWith_For_Query(int take = 20) => new WordsByTopicRequest()
            {
                Skip = 0,
                Take = take,
                SearchTerm = "for",
                Topic = null
            };

            internal static WordsByTopicRequest GetRequestForFirst20WordsWith_For_Query_Def_Topic(int take = 20) => new WordsByTopicRequest()
            {
                Skip = 0,
                Take = take,
                SearchTerm = "for",
                Topic = "def"
            };

            internal static WordsByTopicRequest GetRequestForFirst20WordsWith_Def_Topic(int take = 20) => new WordsByTopicRequest()
            {
                Skip = 0,
                Take = take,
                SearchTerm = null,
                Topic = "def"
            };
        }


        internal static class Command
        {
            internal static WordCreationModel AddWordRequest(string term = null)
            {
                var actualTerm = term == null ? Guid.NewGuid().ToString() : term;
                return new WordCreationModel()
                {
                    Term = actualTerm,
                    Translations = new[] { "bee" },
                    Topic = Constants.DefaultTopic
                };
            }

            internal static WordCreationModel AddWordRequestInvalid(string term = null, string firstTranslation = null, string topic = null)
            {
                return new WordCreationModel()
                {
                    Term = null,
                    Translations = new[] { firstTranslation },
                    Topic = topic
                };
            }

            internal static WordCreationModel AddWordRequestDuplicated()
            {
                return new WordCreationModel()
                {
                    Term = null,
                    Translations = new[] { "bee" },
                    Topic = Constants.DefaultTopic
                };
            }
        }

        internal static class Repetition
        { 
            
        }
       
    }
}