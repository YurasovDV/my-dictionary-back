﻿using DictionaryBack.BL.Query;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.DTOs.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DictionaryReadController : ControllerBase
    {
        private readonly IWordsPagedQueryHandler _pagedHandler;
        private readonly IAllWordsQueryHandler _allWordsQueryHandler;
        private readonly ILogger<DictionaryReadController> _logger;

        public DictionaryReadController(IWordsPagedQueryHandler pagedHandler, IAllWordsQueryHandler allWordsQueryHandler, ILogger<DictionaryReadController> logger)
        {
            _pagedHandler = pagedHandler;
            _allWordsQueryHandler = allWordsQueryHandler;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetNoTracking", Name = "GetNoTracking")]
        [SwaggerResponse(200, type: typeof(OperationResult<IEnumerable<WordDto>>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<OperationResult<IEnumerable<WordDto>>> GetNoTracking()
        {
            return await _allWordsQueryHandler.GetWordsNoTrackingAsync();
        }

        [HttpGet]
        [Route("GetWithDapper", Name = "GetWithDapper")]
        [SwaggerResponse(200, type: typeof(OperationResult<IEnumerable<WordDto>>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<OperationResult<IEnumerable<WordDto>>> GetWithDapper()
        {
            return await _allWordsQueryHandler.GetWithDapperAsync();
        }


        /// <summary>
        /// Only for perfomance comparison
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPage", Name = "GetPage")]
        [SwaggerResponse(200, type: typeof(OperationResult<PageData<WordDto>>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<OperationResult<PageData<WordDto>>> GetPage([FromBody] WordsByTopicRequest request)
        {
            return await _pagedHandler.GetPageTrackingAsync(request);
        }


        [HttpPost]
        [Route("GetPageNoTracking", Name = "GetPageNoTracking")]
        [SwaggerResponse(200, type: typeof(OperationResult<PageData<WordDto>>))]
        public async Task<OperationResult<PageData<WordDto>>> GetPageNoTracking([FromBody] WordsByTopicRequest request)
        {
            return await _pagedHandler.GetPageNoTrackingAsync(request);
        }

        [HttpPost]
        [Route("GetPageDapper", Name = "GetPageDapper")]
        [SwaggerResponse(200, type: typeof(OperationResult<PageData<WordDto>>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<OperationResult<PageData<WordDto>>> GetPageDapper([FromBody] WordsByTopicRequest request)
        {
            return await _pagedHandler.GetPageDapperAsync(request);
        }
    }
}
