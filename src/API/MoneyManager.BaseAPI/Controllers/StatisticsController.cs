using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Middlewaare;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.Statistics.Queries.GetCurrentTotalAmount;
using MoneyManager.Application.Functions.Statistics.Queries.GetTotalForAllCategoryForEachMonthsOfYear;
using MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForAllCategoryOfYear;
using MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForCategoryForEachMonthsOfYear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.API.Controllers
{
    [ApiKeyRequired]
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

        [HttpGet("totalforallcatofyear/{year}")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetTotalsForAllCategoryOfYear([FromRoute] int year)
        {
            return Ok(await _mediator.Send(new GetTotalsForAllCategoryOfYearQuery(_userContextService.GetUserId, year)));
        }

        [HttpGet("totalforallcatforeachmonthofyear/{year}/{month}")]
        public async Task<ActionResult<Dictionary<int, decimal>>> GetTotalForAllCategoryForEachMonthsOfYear([FromRoute] int year, [FromRoute] int month)
        {
            return Ok(await _mediator.Send(new GetTotalForAllCategoryForEachMonthsOfYearQuery(_userContextService.GetUserId, year, month)));
        }
    }
}
