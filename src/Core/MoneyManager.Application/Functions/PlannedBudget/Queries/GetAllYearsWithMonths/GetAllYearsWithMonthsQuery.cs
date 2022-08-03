using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Queries.GetAllYearsWithMonths
{
    public record GetAllYearsWithMonthsQuery(int UserId) : IRequest<Dictionary<int, List<int>>>;
}
