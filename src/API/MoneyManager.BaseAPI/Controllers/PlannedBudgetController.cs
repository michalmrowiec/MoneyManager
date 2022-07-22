using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Commands.DeletePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Commands.UpdatePlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetsForMonth;
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
    [Route("api/plannedbudget")]
    public class PlannedBudgetController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PlannedBudgetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePlannedBudget([FromBody] CreatePlannedBudgetCommand createPlannedBudget)
        {
            createPlannedBudget.UserId = GetUserId();
            var plannedBudget = await _mediator.Send(createPlannedBudget);
            return Created("", plannedBudget);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePlannedBudget([FromBody] UpdatePlannedBudgetCommand updatePlannedBudget)
        {
            updatePlannedBudget.UserId = GetUserId();
            await _mediator.Send(updatePlannedBudget);
            return Ok();
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<List<PlannedBudgetDto>>> GetPlannedBudgetsForMonth([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _mediator.Send(new GetPlannedBudgetsForMonthQuery(GetUserId(), year, month)));
        }

        [HttpGet]
        public async Task<ActionResult<List<PlannedBudgetDto>>> GetAllPlannedBudgets()
        {
            return Ok(await _mediator.Send(new GetAllPlannedBudgetQuery(GetUserId())));
        }

        [HttpDelete("{plannedBudgetId}")]
        public async Task<ActionResult> DeletePlannedBudget([FromRoute] int plannedBudgetId)
        {
            await _mediator.Send(new DeletePlannedBudgetCommand(GetUserId(), plannedBudgetId));
            return NoContent();
        }

        private int GetUserId()
        {
            var f = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return f == null ? 0 : int.Parse(f.Value);
        }
    }
}
