using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.Statistics.Queries.GetCurrentTotalAmount;
using MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForCategoryForEachMonthsOfYear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/stat")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContextService _userContextService;
        public StatisticsController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
        }

        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetCurrentTotalAmount()
        {
            return Ok(await _mediator.Send(new GetCurrentTotalAmountQuery(_userContextService.GetUserId)));
        }

        [HttpGet("totalcatformonths/{categoryId}/{year}")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetTotalsForCategoryForEachMonthsOfYear([FromRoute] int categoryId, [FromRoute] int year)
        {
            return Ok(await _mediator.Send(new GetTotalsForCategoryForEachMonthsOfYearQuery(_userContextService.GetUserId, categoryId, year)));
        }
    }
}
