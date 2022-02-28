using MoneyManager.Server.Commands;
using MoneyManager.Server.Queries;
using MoneyManager.Server.Services;
using MoneyManager.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MoneyManager.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tapi/tracker")]
    public class TrackerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TrackerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecord([FromBody] CreateRecordCommand recordItem)
        {
            await _mediator.Send(recordItem);
            return Created("", null);
        }

        [HttpGet]
        public async Task<ActionResult<List<RecordItemDto>>> GetAllRecords()
        {
            return Ok(await _mediator.Send(new GetAllRecordsQuery()));
        }

        [HttpGet]
        [Route("cat/{categoryId}")]
        public async Task<ActionResult<List<RecordItemDto>>> GetRecordsForCategory([FromRoute] int categoryId)
        {
            return Ok(await _mediator.Send(new GetRecordsForCategoryQuery(categoryId)));
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            await _mediator.Send(new DeleteRecordCommand(recordId));
            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateRecord([FromBody] RecordItemDto recordItem)
        {
            await _mediator.Send(new UpdateRecordCommand(recordItem));
            return Ok();
        }
    }
}
