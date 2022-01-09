using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Query;
using DictionaryBack.Common.Entities;
using DictionaryBack.Common.Localization;
using DictionaryBack.DAL;
using DictionaryBack.DAL.Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Query
{
    public interface IWordsPagedQueryHandler
    {
        Task<OperationResult<PageData<WordDto>>> GetPageTrackingAsync(WordsByTopicRequest request);

        Task<OperationResult<PageData<WordDto>>> GetPageNoTrackingAsync(WordsByTopicRequest request);

        Task<OperationResult<PageData<WordDto>>> GetPageDapperAsync(WordsByTopicRequest request);
    }

    public class WordsPagedQueryHandler : IWordsPagedQueryHandler
    {
        private readonly DictionaryContext _dictionaryContext;
        private readonly IDapperFacade _dapperFacade;
        private readonly ITranslationService _translationService;
        private readonly DictionaryApiSettings _settings;

        public WordsPagedQueryHandler(DictionaryContext dictionaryContext, 
            IDapperFacade dapperFacade, 
            ITranslationService translationService,
            IOptions<DictionaryApiSettings> options)
        {
            _dictionaryContext = dictionaryContext;
            _dapperFacade = dapperFacade;
            _translationService = translationService;
            _settings = options.Value;
        }

        public async Task<OperationResult<PageData<WordDto>>> GetPageTrackingAsync(WordsByTopicRequest request)
        {
            var query = _dictionaryContext.Words.AsQueryable();

            return await ExecuteInternal(query, request);
        }

        public async Task<OperationResult<PageData<WordDto>>> GetPageNoTrackingAsync(WordsByTopicRequest request)
        {
            var query = _dictionaryContext.Words
                .AsNoTracking()
                .AsQueryable();

            return await ExecuteInternal(query, request);
        }

        public async Task<OperationResult<PageData<WordDto>>> GetPageDapperAsync(WordsByTopicRequest request)
        {
            try
            {
                var wordsPage = await _dapperFacade.GetPage(request);
                PageData<WordDto> result = PageDataExt.From(wordsPage, w => Mapper.Map(w));
                return OperationResultExt.Success(result);
            }
            catch (Exception ex)
            {
                // TODO log
                return OperationResultExt.Fail<PageData<WordDto>>(CommandStatus.InternalError, ex.Message);
            }
        }


        private async Task<OperationResult<PageData<WordDto>>> ExecuteInternal(IQueryable<Word> query, WordsByTopicRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(w => w.Term.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.Topic))
            {
                query = query.Where(w => w.Topic.Name.Contains(request.Topic));
            }

            if(request.Take != null)
            {
                if (request.Take > _settings.MaxWordsInRequest)
                {
                    return OperationResultExt.Fail<PageData<WordDto>>(CommandStatus.InvalidRequest, _translationService.GetTranslation(ErrorKey.TooManyItemsRequested));
                }
            }

            var count = await query.CountAsync();
            IEnumerable<WordDto> data = Array.Empty<WordDto>();

            if (count > 0)
            {
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
                    else
                    {
                        query = query.Take(_settings.MaxWordsInRequest);
                    }
                }

                data = await query.Select(w => Mapper.Map(w)).ToListAsync();
            }

            return OperationResultExt.Success(new PageData<WordDto>() { Total = count, Page = data.ToArray() });
        }
    }
}
