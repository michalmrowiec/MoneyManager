using MediatR;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllPlannedBudget;

namespace MoneyManager.Application.Functions.Records.Queries.GetAllYearsWithMonths
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
            var all = await _mediator.Send(new GetAllRecordsQuery(request.UserId));

            var years = all
                .Select(x => x.TransactionDate.Year)
                .Distinct();

            Dictionary<int, List<int>> result = new();

            foreach (var year in years)
                result.Add(year, all
                    .Select(x => x.TransactionDate)
                    .Where(x => x.Year == year)
                    .Select(x => x.Month)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList());

            return result;
        }
    }
}
