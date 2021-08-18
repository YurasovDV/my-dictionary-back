using DictionaryBack.BL.Command;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]")]
    public class RepetitionController : ControllerBase
    {

        private readonly IRepetitionHandler _repetitionHandler;
        private readonly ILogger<RepetitionController> _logger;

        public RepetitionController(IRepetitionHandler repetitionHandler,
            ILogger<RepetitionController> logger)
        {
            _repetitionHandler = repetitionHandler;
            _logger = logger;
        }

        [HttpPost]
        [Route("", Name = "CreateRepetitionSet")]
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto[]>> CreateRepetitionSet()
        {
            return await _repetitionHandler.CreateSet();
        }
    }
}
