using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Commands.DeletePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Commands.UpdatePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllYearsWithMonths;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetById;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetsForMonth;

namespace MoneyManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/plannedbudget")]
    public class PlannedBudgetController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContextService _userContextService;
        public PlannedBudgetController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlannedBudget([FromBody] CreatePlannedBudgetCommand createPlannedBudget)
        {
            createPlannedBudget.UserId = _userContextService.GetUserId;
            var plannedBudget = await _mediator.Send(createPlannedBudget);
            return Created("", plannedBudget);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePlannedBudget([FromBody] UpdatePlannedBudgetCommand updatePlannedBudget)
        {
            updatePlannedBudget.UserId = _userContextService.GetUserId;
            await _mediator.Send(updatePlannedBudget);
            return Ok();
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<List<PlannedBudgetDto>>> GetPlannedBudgetsForMonth([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _mediator.Send(new GetPlannedBudgetsForMonthQuery(_userContextService.GetUserId, year, month)));
        }

        [HttpGet]
        public async Task<ActionResult<List<PlannedBudgetDto>>> GetAllPlannedBudgets()
        {
            return Ok(await _mediator.Send(new GetAllPlannedBudgetQuery(_userContextService.GetUserId)));
        }

        [HttpGet("{plannedBudgetId}")]
        public async Task<ActionResult<PlannedBudgetDto>> GetRecordById([FromRoute] int plannedBudgetId)
        {
            return Ok(await _mediator.Send(new GetPlannedBudgetByIdQuery(_userContextService.GetUserId, plannedBudgetId)));
        }

        [HttpGet("dates")]
        public async Task<ActionResult<Dictionary<int, List<int>>>> GetAllYearsWithMonths()
        {
            return Ok(await _mediator.Send(new GetAllYearsWithMonthsQuery(_userContextService.GetUserId)));
        }

        [HttpDelete("{plannedBudgetId}")]
        public async Task<ActionResult> DeletePlannedBudget([FromRoute] int plannedBudgetId)
        {
            await _mediator.Send(new DeletePlannedBudgetCommand(_userContextService.GetUserId, plannedBudgetId));
            return NoContent();
        }
    }
}
