﻿using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    public interface IWordsByTopicQueryHandler
    {
        Task<OperationResult<IEnumerable<WordDto>>> GetPageTrackingAsync(WordsByTopicRequest request);

        Task<OperationResult<IEnumerable<WordDto>>> GetPageNoTracking(WordsByTopicRequest request);

        Task<OperationResult<IEnumerable<WordDto>>> GetPageDapper(WordsByTopicRequest request);
    }

    public class WordsByTopicQueryHandler : IWordsByTopicQueryHandler
    {
        private readonly DictionaryContext _dictionaryContext;
        private readonly IDapperFacade _dapperFacade;

        public WordsByTopicQueryHandler(DictionaryContext dictionaryContext, IDapperFacade dapperFacade)
        {
            _dictionaryContext = dictionaryContext;
            _dapperFacade = dapperFacade;
        }

        public async Task<OperationResult<IEnumerable<WordDto>>> GetPageTrackingAsync(WordsByTopicRequest request)
        {
            var query = _dictionaryContext.Words.AsQueryable();

            return await ExecuteInternal(query, request);
        }

        public async Task<OperationResult<IEnumerable<WordDto>>> GetPageNoTracking(WordsByTopicRequest request)
        {
            var query = _dictionaryContext.Words
                .AsNoTracking()
                .AsQueryable();

            return await ExecuteInternal(query, request);
        }

        public async Task<OperationResult<IEnumerable<WordDto>>> GetPageDapper(WordsByTopicRequest request)
        {
            IEnumerable<WordDto> data = (await _dapperFacade.GetPage(request))
                .Select(w => Mapper.Map(w))
                .ToList();

            return OperationResultExt.Success(data);
        }


        private async Task<OperationResult<IEnumerable<WordDto>>> ExecuteInternal(IQueryable<Word> query, WordsByTopicRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(w => w.Term.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.Topic))
            {
                query = query.Where(w => w.Topic.Name.Contains(request.Topic));
            }

            if (request.Skip != null || request.Take != null)
            {
                query = query.OrderBy(w => w.Term);
                if (request.Skip != null)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Take != null && request.Take > 0)
                {
                    query = query.Take(request.Take.Value);
                }
            }

            IEnumerable<WordDto> data = await query.Select(w => Mapper.Map(w)).ToListAsync();
            return OperationResultExt.Success(data);
        }
    }
}
