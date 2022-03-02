using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.BaseAPI.Controllers
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
            await _mediator.Send(recordItem);
            return Created("", null);
        }

        [HttpGet]
        public async Task<ActionResult<List<RecordDto>>> GetAllRecords()
        {
            return Ok(await _mediator.Send(new GetAllRecordsQuery(GetUserId())));
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

        [HttpPatch]
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
