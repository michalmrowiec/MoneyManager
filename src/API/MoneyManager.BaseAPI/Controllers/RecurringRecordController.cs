using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Commands.DeleteRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords;
using MoneyManager.Application.Functions.RecurringRecords.Commands.UpdateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Queries;
using MoneyManager.Application.Functions.RecurringRecords.Queries.GetAllRecurringRecords;
using MoneyManager.Application.Functions.RecurringRecords.Queries.GetRecurringRecordById;
using System.Security.Claims;

namespace MoneyManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/recurring")]
    public class RecurringRecordController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RecurringRecordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecurringRecord([FromBody] CreateRecurringRecordCommand createRecurringRecord)
        {
            createRecurringRecord.UserId = GetUserId();
            await _mediator.Send(createRecurringRecord);
            return Created("", null);
        }

        [HttpGet("{recurringRecordId}")]
        public async Task<ActionResult<RecurringRecordDto>> GetRecordById([FromRoute] int recurringRecordId)
        {
            return Ok(await _mediator.Send(new GetRecurringRecordByIdQuery(GetUserId(), recurringRecordId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<RecurringRecordDto>>> GetlAllRecurringRecords()
        {
            return Ok(await _mediator.Send(new GetAllRecurringRecordsQuery(GetUserId())));
        }

        [HttpGet]
        [Route("ex/{date?}")]
        public async Task<ActionResult> ExecuteRecurringRecords([FromRoute] string? date = null)
        {
            DateTime dateTime = date is null ? DateTime.Now : DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            return Ok(await _mediator.Send(new ExecuteRecurringRecordsCommand(GetUserId(), dateTime)));
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateRecurringRecord([FromBody] UpdateRecurringRecordCommand updateRecurringRecord)
        {
            updateRecurringRecord.UserId = GetUserId();
            await _mediator.Send(updateRecurringRecord);
            return Ok();
        }

        [HttpDelete("{recurringRecordId}")]
        public async Task<ActionResult> DeleteRecurringRecord([FromRoute] int recurringRecordId)
        {
            await _mediator.Send(new DeleteRecurringRecordCommand(GetUserId(), recurringRecordId));
            return NoContent();
        }

        private int GetUserId()
        {
            var f = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return f == null ? 0 : int.Parse(f.Value);
        }
    }
}
