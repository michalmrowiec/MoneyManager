using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Middlewaare;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Commands.DeleteRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords;
using MoneyManager.Application.Functions.RecurringRecords.Commands.UpdateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Queries;
using MoneyManager.Application.Functions.RecurringRecords.Queries.GetAllRecurringRecords;
using MoneyManager.Application.Functions.RecurringRecords.Queries.GetRecurringRecordById;

namespace MoneyManager.API.Controllers
{
    [ApiKeyRequired]
    [Authorize]
    [ApiController]
    [Route("api/recurring")]
    public class RecurringRecordController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContextService _userContextService;
        public RecurringRecordController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecurringRecord([FromBody] CreateRecurringRecordCommand createRecurringRecord)
        {
            createRecurringRecord.UserId = _userContextService.GetUserId;
            await _mediator.Send(createRecurringRecord);
            return Created("", null);
        }

        [HttpGet("{recurringRecordId}")]
        public async Task<ActionResult<RecurringRecordDto>> GetRecordById([FromRoute] int recurringRecordId)
        {
            return Ok(await _mediator.Send(new GetRecurringRecordByIdQuery(_userContextService.GetUserId, recurringRecordId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<RecurringRecordDto>>> GetlAllRecurringRecords()
        {
            return Ok(await _mediator.Send(new GetAllRecurringRecordsQuery(_userContextService.GetUserId)));
        }

        [HttpGet]
        [Route("ex/{date?}")]
        public async Task<ActionResult> ExecuteRecurringRecords([FromRoute] string? date = null)
        {
            DateTime dateTime = date is null ? DateTime.Now : DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            return Ok(await _mediator.Send(new ExecuteRecurringRecordsCommand(_userContextService.GetUserId, dateTime)));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRecurringRecord([FromBody] UpdateRecurringRecordCommand updateRecurringRecord)
        {
            updateRecurringRecord.UserId = _userContextService.GetUserId;
            await _mediator.Send(updateRecurringRecord);
            return Ok();
        }

        [HttpDelete("{recurringRecordId}")]
        public async Task<ActionResult> DeleteRecurringRecord([FromRoute] int recurringRecordId)
        {
            await _mediator.Send(new DeleteRecurringRecordCommand(_userContextService.GetUserId, recurringRecordId));
            return NoContent();
        }
    }
}
