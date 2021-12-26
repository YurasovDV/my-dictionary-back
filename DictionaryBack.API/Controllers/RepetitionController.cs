using DictionaryBack.BL.Command;
using DictionaryBack.Infrastructure;
using DictionaryBack.Infrastructure.DTOs.Command;
using DictionaryBack.Infrastructure.DTOs.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
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
            _logger.LogInformation("{operationName} | Start creating repetition set", nameof(CreateRepetitionSet));
            var setCreationResult = await _repetitionHandler.CreateSet();
            if (setCreationResult.IsSuccessful())
            {
                _logger.LogInformation("{operationName} | Created repetition set | {setSize}", nameof(CreateRepetitionSet), setCreationResult.Data.Length);
            }
            else
            {
                _logger.LogWarning("{operationName} | Could not create repetition set | {error}", nameof(CreateRepetitionSet), setCreationResult.ErrorText);
            }
            return setCreationResult;
        }

        [HttpPost]
        [Route("CompleteRepetition", Name = "CompleteRepetition")]
        [SwaggerResponse(200, type: typeof(BoolOperationResult))]
        public async Task<BoolOperationResult> CompleteRepetition(WordRepetitionResult[] words)
        {

            _logger.LogInformation("{operationName} | Start completing repetition set | {setCount}", nameof(CompleteRepetition), words.Length);
            LogWholeSetDebug(words);
            var completionResult = await _repetitionHandler.CompleteRepetition(words);
            if (completionResult.IsSuccessful())
            {
                _logger.LogInformation("{operationName} | Completed repetition set", nameof(CompleteRepetition));
            }
            else
            {
                _logger.LogWarning("{operationName} | Could not complete repetition set | {error}", nameof(CompleteRepetition), completionResult.ErrorText);
            }
            return completionResult;
        }

        private void LogWholeSetDebug(WordRepetitionResult[] words)
        {
            _logger.LogDebug("{operationName} | {set}", nameof(CompleteRepetition), string.Concat(words.Select(w => w.ToString()), ", "));
        }
    }
}
