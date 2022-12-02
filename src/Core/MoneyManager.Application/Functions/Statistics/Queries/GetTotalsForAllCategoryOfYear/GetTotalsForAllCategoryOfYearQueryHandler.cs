using MediatR;
using MoneyManager.Application.Functions.Records;
using System.Security.Cryptography.X509Certificates;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForAllCategoryOfYear
{
    public class GetTotalsForAllCategoryOfYearQueryHandler : IRequestHandler<GetTotalsForAllCategoryOfYearQuery, Dictionary<int, decimal>>
    {
        private readonly IMediator _mediator;

        public GetTotalsForAllCategoryOfYearQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Dictionary<int, decimal>> Handle(GetTotalsForAllCategoryOfYearQuery request, CancellationToken cancellationToken)
        {
            var records = await _mediator.Send(new GetAllRecordsQuery(request.UserId));

            return records.Where(x => x.TransactionDate.Year == request.Year && x.CategoryId != null)
                .GroupBy(x => (int)x.CategoryId)
                .ToDictionary(x => x.Key, x => x.Sum(x => x.Amount));
        }
    }
}