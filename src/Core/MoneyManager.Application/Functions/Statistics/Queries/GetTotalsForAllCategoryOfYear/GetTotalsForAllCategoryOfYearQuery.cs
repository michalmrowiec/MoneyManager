using MediatR;

namespace MoneyManager.Application.Functions.Statistics.Queries.GetTotalsForAllCategoryOfYear
{
    public record GetTotalsForAllCategoryOfYearQuery(int UserId, int Year) : IRequest<Dictionary<int, decimal>>;
}
