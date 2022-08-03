using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetsForMonth
{
    public record GetPlannedBudgetsForMonthQuery(int UserId, int Year, int Month) : IRequest<List<PlannedBudgetDto>>;
}
