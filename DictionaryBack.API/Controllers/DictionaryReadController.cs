using DictionaryBack.BL.Query;
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
        private readonly IWordsByTopicQueryHandler _topicHandler;
        private readonly IAllWordsQueryHandler _allWordsQueryHandler;
        private readonly ILogger<DictionaryReadController> _logger;

        public DictionaryReadController(IWordsByTopicQueryHandler topicHandler, IAllWordsQueryHandler allWordsQueryHandler, ILogger<DictionaryReadController> logger)
        {
            _topicHandler = topicHandler;
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
        [SwaggerResponse(200, type: typeof(OperationResult<IEnumerable<WordDto>>))]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<OperationResult<IEnumerable<WordDto>>> GetPage([FromBody] WordsByTopicRequest request)
        {
            return await _topicHandler.GetPageTrackingAsync(request);
        }


        [HttpPost]
        [Route("GetPageNoTracking", Name = "GetPageNoTracking")]
        [SwaggerResponse(200, type: typeof(OperationResult<IEnumerable<WordDto>>))]
        public async Task<OperationResult<IEnumerable<WordDto>>> GetPageNoTracking([FromBody] WordsByTopicRequest request)
        {
            return await _topicHandler.GetPageNoTrackingAsync(request);
        }

        [HttpPost]
        [Route("GetPageDapper", Name = "GetPageDapper")]
        [SwaggerResponse(200, type: typeof(OperationResult<IEnumerable<WordDto>>))]
        // TODO: use SoftDeleted column in dapper queries
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<OperationResult<IEnumerable<WordDto>>> GetPageDapper([FromBody] WordsByTopicRequest request)
        {
            return await _topicHandler.GetPageDapperAsync(request);
        }
    }
}
