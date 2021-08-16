using DictionaryBack.BL;
using DictionaryBack.BL.Query;
using DictionaryBack.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly IWordsByTopicQueryHandler _topicHandler;
        private readonly IAllWordsQueryHandler _allWordsQueryHandler;
        private readonly ILogger<DictionaryController> _logger;

        public DictionaryController(IWordsByTopicQueryHandler topicHandler, IAllWordsQueryHandler allWordsQueryHandler, ILogger<DictionaryController> logger)
        {
            _topicHandler = topicHandler;
            _allWordsQueryHandler = allWordsQueryHandler;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Word>> Get()
        {
            return await _allWordsQueryHandler.GetWordsAsync();
        }

        [HttpGet]
        [Route("getPage")]
        public async Task<IEnumerable<Word>> GetPage(WordsByTopicRequest request)
        {
            return await _topicHandler.GetWordsAsync(request);
        }
    }
}
