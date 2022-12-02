using MediatR;
using MoneyManager.Application.Functions.Records;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForCategoryForEachMonthsOfYear
{
    public class GetTotalsForCategoryForEachMonthsOfYearQueryHandler : IRequestHandler<GetTotalsForCategoryForEachMonthsOfYearQuery, Dictionary<int, decimal>>
    {
        private readonly IMediator _mediator;
        public GetTotalsForCategoryForEachMonthsOfYearQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Dictionary<int, decimal>> Handle(GetTotalsForCategoryForEachMonthsOfYearQuery request, CancellationToken cancellationToken)
        {
            var records = await _mediator.Send(new GetAllRecordsQuery(request.UserId));

            return records.Where(x => x.CategoryId == request.CategoryId && x.TransactionDate.Year == request.Year)
                .GroupBy(x => x.TransactionDate.Month)
                .ToDictionary(x => x.Key, x => x.Sum(x => x.Amount));
        }
    }
}
