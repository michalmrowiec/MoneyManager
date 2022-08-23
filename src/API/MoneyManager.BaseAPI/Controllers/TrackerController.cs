using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.Records;
using MoneyManager.Application.Functions.Records.Commands.CreateRangeRecords;
using MoneyManager.Application.Functions.Records.Queries.GetRecordById;
using MoneyManager.Application.Functions.Records.Queries.GetRecordsForMonth;
using MoneyManager.Domain.Entities;
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
        private readonly IUserContextService _userContextService;
        public TrackerController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecord([FromBody] CreateRecordCommand recordItem)
        {
            recordItem.UserId = _userContextService.GetUserId;
            var record = await _mediator.Send(recordItem);
            return Created("", record);
        }

        [HttpGet]
        public async Task<ActionResult<List<RecordDto>>> GetAllRecords()
        {
            return Ok(await _mediator.Send(new GetAllRecordsQuery(_userContextService.GetUserId)));
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<List<RecordDto>>> GetRecordsForMonth([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _mediator.Send(new GetRecordsForMonthQuery(_userContextService.GetUserId, year, month)));
        }

        [HttpGet("{recordId}")]
        public async Task<ActionResult<RecordDto>> GetRecordById([FromRoute] int recordId)
        {
            return Ok(await _mediator.Send(new GetRecordByIdQuery(_userContextService.GetUserId, recordId)));
        }

        [HttpGet]
        [Route("cat/{categoryId}")]
        public async Task<ActionResult<List<RecordDto>>> GetRecordsForCategory([FromRoute] int categoryId)
        {
            return Ok(await _mediator.Send(new GetRecordsForCategoryQuery(_userContextService.GetUserId, categoryId)));
        }

        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord([FromRoute] int recordId)
        {
            await _mediator.Send(new DeleteRecordCommand(_userContextService.GetUserId, recordId));
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRecord([FromBody] UpdateRecordCommand recordItem)
        {
            recordItem.UserId = _userContextService.GetUserId;
            await _mediator.Send(recordItem);
            return Ok();
        }
    }
}
