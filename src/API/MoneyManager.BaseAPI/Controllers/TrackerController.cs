using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.Records.Queries.GetRecordById;
using MoneyManager.Application.Functions.Records.Queries.GetRecordsForMonth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tracker")]
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
            recordItem.UserId = GetUserId();
            var record = await _mediator.Send(recordItem);
            return Created("", record);
        }

        [HttpGet]
        public async Task<ActionResult<List<RecordDto>>> GetAllRecords()
        {
            return Ok(await _mediator.Send(new GetAllRecordsQuery(GetUserId())));
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<List<RecordDto>>> GetRecordsForMonth([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _mediator.Send(new GetRecordsForMonthQuery(GetUserId(), year, month)));
        }

        [HttpGet("{recordId}")]
        public async Task<ActionResult<RecordDto>> GetRecordById([FromRoute] int recordId)
        {
            return Ok(await _mediator.Send(new GetRecordByIdQuery(GetUserId(), recordId)));
        }

        [HttpGet]
        [Route("cat/{categoryId}")]
        public async Task<ActionResult<List<RecordDto>>> GetRecordsForCategory([FromRoute] int categoryId)
        {
            return Ok(await _mediator.Send(new GetRecordsForCategoryQuery(GetUserId(), categoryId)));
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            await _mediator.Send(new DeleteRecordCommand(GetUserId(), recordId));
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRecord([FromBody] UpdateRecordCommand recordItem)
        {
            recordItem.UserId = GetUserId();
            await _mediator.Send(recordItem);
            return Ok();
        }

        private int GetUserId()
        {
            var f = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return f == null ? 0 : int.Parse(f.Value);
        }
    }
}
