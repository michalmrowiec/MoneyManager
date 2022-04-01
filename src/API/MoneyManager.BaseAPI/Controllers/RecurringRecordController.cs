using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord;
using MoneyManager.Application.Functions.RecurringRecords.Commands.ExecuteRecurringRecords;
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
        public async Task<IActionResult> CreateRecurringRecord([FromBody] CreateRecurringRecordCommand createRecurringRecord)
        {
            createRecurringRecord.UserId = GetUserId();
            await _mediator.Send(createRecurringRecord);
            return Created("", null);
        }

        [HttpGet]
        public async Task<ActionResult> ExecuteRecurringRecords()
        {
            return Ok(await _mediator.Send(new ExecuteRecurringRecordsCommand(GetUserId())));
        }

        private int GetUserId()
        {
            var f = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return f == null ? 0 : int.Parse(f.Value);
        }
    }
}
