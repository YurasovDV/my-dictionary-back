using DictionaryBack.BL.Command;
using DictionaryBack.BL.Command.Models;
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
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto>> AddWord([FromBody] WordCreationModel request)
        {
            return await _createHandler.Create(request);
        }

        [HttpPut]
        [SwaggerResponse(200, type: typeof(OperationResult<WordDto>))]
        public async Task<OperationResult<WordDto>> EditWord([FromBody] WordEditModel request)
        {
            return await _editHandler.Edit(request);
        }

        [HttpDelete]
        [SwaggerResponse(200, type: typeof(BoolOperationResult))]
        public async Task<BoolOperationResult> DeleteWord(string term)
        {
            return await _deleteHandler.Delete(term);
        }
    }
}
