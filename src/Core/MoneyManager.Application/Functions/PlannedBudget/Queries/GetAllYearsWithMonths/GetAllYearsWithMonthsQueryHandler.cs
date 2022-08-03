using MediatR;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllYearsWithMonths
{
    public class GetAllYearsWithMonthsQueryHandler : IRequestHandler<GetAllYearsWithMonthsQuery, Dictionary<int, List<int>>>
    {
        private readonly IMediator _mediator;
        public GetAllYearsWithMonthsQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Dictionary<int, List<int>>> Handle(GetAllYearsWithMonthsQuery request, CancellationToken cancellationToken)
        {
            var all = await _mediator.Send(new GetAllPlannedBudgetQuery(request.UserId));

            var years = all.Select(x => x.PlanForMonth.Year).Distinct();

            Dictionary<int, List<int>> result = new();

            foreach (var year in years)
                result.Add(year, all.Select(x => x.PlanForMonth).Where(x => x.Year == year).Select(x => x.Month).Distinct().OrderBy(x => x).ToList());

            return result;
        }
    }
}
