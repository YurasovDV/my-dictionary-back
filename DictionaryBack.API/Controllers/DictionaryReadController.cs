using DictionaryBack.BL.Query;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Infrastructure.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
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

        // debug version
        [HttpGet]
        [Route("", Name = "GetWordsAsync")]
        public async Task<IEnumerable<WordDto>> Get()
        {
            return await _allWordsQueryHandler.GetWordsAsync();
        }

        // debug version
        [HttpGet]
        [Route("GetNoTracking", Name = "GetNoTracking")]
        public async Task<IEnumerable<WordDto>> GetNoTracking()
        {
            return await _allWordsQueryHandler.GetWordsNoTrackingAsync();
        }

        [HttpGet]
        [Route("GetWithDapper", Name = "GetWithDapper")]
        public async Task<IEnumerable<WordDto>> GetWithDapper()
        {
            return await _allWordsQueryHandler.GetWithDapper();
        }


        [HttpPost]
        [Route("GetPage", Name = "GetPage")]
        public async Task<IEnumerable<WordDto>> GetPage([FromBody] WordsByTopicRequest request)
        {
            return await _topicHandler.GetPageTrackingAsync(request);
        }


        [HttpPost]
        [Route("GetPageNoTracking", Name = "GetPageNoTracking")]
        public async Task<IEnumerable<WordDto>> GetPageNoTracking([FromBody] WordsByTopicRequest request)
        {
            return await _topicHandler.GetPageNoTracking(request);
        }

        [HttpPost]
        [Route("GetPageDapper", Name = "GetPageDapper")]
        public async Task<IEnumerable<WordDto>> GetPageDapper([FromBody] WordsByTopicRequest request)
        {
            return await _topicHandler.GetPageDapper(request);
        }
    }
}
