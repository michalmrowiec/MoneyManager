using MediatR;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetById
{
    public record GetPlannedBudgetByIdQuery(int UserId, int PlannedBudgetId) : IRequest<PlannedBudgetDto>;
}
