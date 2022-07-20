using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
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
        public async Task<ActionResult> CreateCategory([FromBody] CreatePlannedBudgetCommand createPlannedBudget)
        {
            createPlannedBudget.UserId = GetUserId();
            var plannedBudget = await _mediator.Send(createPlannedBudget);
            return Created("", plannedBudget);
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<List<PlannedBudgetDto>>> GetPlannedBudgetsForMonth([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _mediator.Send(new GetPlannedBudgetsForMonthQuery(GetUserId(), year, month)));
        }

        private int GetUserId()
        {
            var f = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return f == null ? 0 : int.Parse(f.Value);
        }
    }
}
