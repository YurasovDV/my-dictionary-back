using DictionaryBack.BL;
using DictionaryBack.DAL;
using DictionaryBack.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly DictionaryContext _context;
        private readonly ILogger<DictionaryController> _logger;

        public DictionaryController(DictionaryContext context, ILogger<DictionaryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Word> Get()
        {
            return _context.Words.ToList();
        }

        [HttpGet]
        [Route("getPage")]
        public IEnumerable<Word> GetPage(int skip = 0, int take = 20)
        {
            return _context.Words.Skip(skip).Take(take);
        }

    }
}
