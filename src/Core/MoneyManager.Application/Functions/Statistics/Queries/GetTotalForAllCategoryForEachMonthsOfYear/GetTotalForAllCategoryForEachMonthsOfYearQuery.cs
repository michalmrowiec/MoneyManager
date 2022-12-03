using MediatR;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalForAllCategoryForEachMonthsOfYear
{
    public record GetTotalForAllCategoryForEachMonthsOfYearQuery(int UserId, int Year, int Month) : IRequest<Dictionary<int, decimal>>;
}
