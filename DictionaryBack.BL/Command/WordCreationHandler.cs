﻿using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordCreationHandler
    {
        Task<OperationResult<WordDto>> Create(WordCreationModel request);
    }

    public class WordCreationHandler : BaseCommand, IWordCreationHandler
    {
        public WordCreationHandler(DictionaryContext dictionaryContext) : base(dictionaryContext) { }

        public async Task<OperationResult<WordDto>> Create(WordCreationModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Term))
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, "No word provided");
                }

                if (request.Translations == null || request.Translations.Length == 0 || request.Translations.Any(r => string.IsNullOrWhiteSpace(r)))
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, "Incorrect translations");
                }

                var word = Mapper.Map(request);

                var existingTopic = TopicFinder.FindTopic(word.Topic.Name);
                if (existingTopic.IsSuccessful())
                {
                    word.TopicId = existingTopic.Data.Id;
                    word.Topic = existingTopic.Data;
                }
                else
                {
                    return OperationResultExt.Fail<WordDto>(CommandStatus.InvalidRequest, "Incorrect topic name");
                }

                DictionaryContext.Words.Add(word);
                await DictionaryContext.SaveChangesAsync();
                return OperationResultExt.Success(Mapper.Map(word));
            }
            catch (Exception)
            {
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, "Internal error");
            }
        }
    }
}
