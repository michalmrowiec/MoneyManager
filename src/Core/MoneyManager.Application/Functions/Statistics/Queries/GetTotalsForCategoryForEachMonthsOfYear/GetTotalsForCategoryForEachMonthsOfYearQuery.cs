using MediatR;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForCategoryForEachMonthsOfYear
{
    public record GetTotalsForCategoryForEachMonthsOfYearQuery(int UserId, int CategoryId, int Year) : IRequest<Dictionary<int, decimal>>;
}
