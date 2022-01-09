using DictionaryBack.BL.Command;
using DictionaryBack.Common;
using DictionaryBack.Common.DTOs.Command;
using DictionaryBack.Common.DTOs.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace DictionaryBack.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DictionaryCommandController : ControllerBase
    {
        private readonly IWordCreationHandler _createHandler;
        private readonly IWordEditHandler _editHandler;
        private readonly IWordDeletionHandler _deleteHandler;
        private readonly ILogger<DictionaryCommandController> _logger;

        public DictionaryCommandController(IWordCreationHandler create, 
            IWordEditHandler edit, 
            IWordDeletionHandler delete, 
            ILogger<DictionaryCommandController> logger)
        {
            _createHandler = create;
            _editHandler = edit;
            _deleteHandler = delete;
            _logger = logger;
        }

        [HttpPost]
        [Route("", Name = "AddWord")]
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto>> AddWord([FromBody] WordCreationModel request)
        {
            _logger.LogInformation("{operationName} | Start adding word | @{request}", nameof(AddWord), request);
            var addedResult = await _createHandler.Create(request);
            if (addedResult.IsSuccessful())
            {
                _logger.LogInformation("{operationName} | Completed adding word | @{request}", nameof(AddWord), request);
            }
            else
            {
                _logger.LogWarning("{operationName} | Could not edit word | {Status} , {error}", nameof(AddWord), addedResult.StatusCode, addedResult.ErrorText);

            }
            return addedResult;
        }

        [HttpPut]
        [Route("", Name = "EditWord")]
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto>> EditWord([FromBody] WordEditModel request)
        {
            _logger.LogInformation("{operationName} | Start editing word | {word}", nameof(EditWord), request.Term);
            var editResult = await _editHandler.Edit(request);
            if (editResult.IsSuccessful())
            {
                _logger.LogInformation("{operationName} | Completed editing word | @{request}, {status}, {error}", nameof(EditWord), request, editResult.StatusCode, editResult.ErrorText);
            }
            else
            {
                _logger.LogWarning("{operationName} | Could not edit word | {Status} , {error}", nameof(EditWord), editResult.StatusCode, editResult.ErrorText);
            }
            return editResult;
        }

        [HttpDelete]
        [Route("{term}", Name = "DeleteWord")]
        [SwaggerResponse(200, type: typeof(BoolOperationResult))]
        public async Task<BoolOperationResult> DeleteWord(string term)
        {
            _logger.LogInformation("{operationName} | Start deleting word | {word}", nameof(DeleteWord), term);
            var deletedResult = await _deleteHandler.Delete(term);
            if (deletedResult.IsSuccessful())
            {
                _logger.LogInformation("{operationName} | Completed deleting word | {word}", nameof(DeleteWord), term, deletedResult.StatusCode, deletedResult.ErrorText);

            }
            else
            {
                _logger.LogWarning("{operationName} | Could not delete word | {word} , {Status} , {error}", nameof(DeleteWord), term, deletedResult.StatusCode, deletedResult.ErrorText);
            }

            return deletedResult;
        }
    }
}
