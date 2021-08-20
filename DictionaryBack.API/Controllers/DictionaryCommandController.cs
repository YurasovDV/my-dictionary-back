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
            return await _createHandler.Create(request);
        }

        [HttpPut]
        [Route("", Name = "EditWord")]
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto>> EditWord([FromBody] WordEditModel request)
        {
            return await _editHandler.Edit(request);
        }

        [HttpDelete]
        [Route("{term}", Name = "DeleteWord")]
        [SwaggerResponse(200, type: typeof(BoolOperationResult))]
        public async Task<BoolOperationResult> DeleteWord(string term)
        {
            return await _deleteHandler.Delete(term);
        }
    }
}
