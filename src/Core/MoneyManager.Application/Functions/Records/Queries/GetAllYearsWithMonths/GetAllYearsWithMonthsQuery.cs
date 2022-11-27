using MediatR;

namespace MoneyManager.Application.Functions.Records.Queries.GetAllYearsWithMonths
{
    public record GetAllYearsWithMonthsQuery(int UserId) : IRequest<Dictionary<int, List<int>>>;
}
