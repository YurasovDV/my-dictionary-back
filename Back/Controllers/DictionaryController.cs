using DictionaryBack.BL;
using DictionaryBack.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DictionaryBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _readService;
        private readonly ILogger<DictionaryController> _logger;

        public DictionaryController(IDictionaryService readService, ILogger<DictionaryController> logger)
        {
            _readService = readService;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Word> Get()
        {
            return _readService.All();
        }

        [HttpGet]
        [Route("getPage")]
        public IEnumerable<Word> GetPage(int skip = 0, int take = 20)
        {
            return _readService.Take(skip, take);
        }

    }
}
