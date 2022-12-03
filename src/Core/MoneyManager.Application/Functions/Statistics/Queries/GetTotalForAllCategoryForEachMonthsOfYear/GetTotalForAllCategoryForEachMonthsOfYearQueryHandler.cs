using MediatR;
using MoneyManager.Application.Functions.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalForAllCategoryForEachMonthsOfYear
{
    public class GetTotalForAllCategoryForEachMonthsOfYearQueryHandler : IRequestHandler<GetTotalForAllCategoryForEachMonthsOfYearQuery, Dictionary<int, decimal>>
    {
        private readonly IMediator _mediator;

        public GetTotalForAllCategoryForEachMonthsOfYearQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Dictionary<int, decimal>> Handle(GetTotalForAllCategoryForEachMonthsOfYearQuery request, CancellationToken cancellationToken)
        {
            var records = await _mediator.Send(new GetAllRecordsQuery(request.UserId));

            return records.Where(x => x.TransactionDate.Year == request.Year && x.TransactionDate.Month == request.Month && x.CategoryId != null)
                .GroupBy(x => (int)x.CategoryId)
                .ToDictionary(x => x.Key, x => x.Sum(x => x.Amount));
        }
    }
}
