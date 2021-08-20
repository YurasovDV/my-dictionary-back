using DictionaryBack.BL.Command;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.DTOs.Command;
using DictionaryBack.Infrastructure.DTOs.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [Route("CreateRepetitionSet", Name = "CreateRepetitionSet")]
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto[]>> CreateRepetitionSet()
        {
            return await _repetitionHandler.CreateSet();
        }

        [HttpPost]
        [Route("CompleteRepetition", Name = "CompleteRepetition")]
        [SwaggerResponse(200, type: typeof(BoolOperationResult))]
        public async Task<BoolOperationResult> CompleteRepetition(WordRepetitionResult[] words)
        {
            return await _repetitionHandler.CompleteRepetition(words);
        }
    }
}
