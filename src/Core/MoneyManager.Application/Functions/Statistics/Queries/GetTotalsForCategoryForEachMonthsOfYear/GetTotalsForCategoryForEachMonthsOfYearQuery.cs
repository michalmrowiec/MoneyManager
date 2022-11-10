using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForCategoryForEachMonthsOfYear
{
    public record GetTotalsForCategoryForEachMonthsOfYearQuery(int UserId, int CategoryId, int Year) : IRequest<Dictionary<int, decimal>>;
}
