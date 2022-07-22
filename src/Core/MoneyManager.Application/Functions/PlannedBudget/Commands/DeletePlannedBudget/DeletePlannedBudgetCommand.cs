using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Commands.DeletePlannedBudget
{
    public record DeletePlannedBudgetCommand(int UserId, int PlannedBudgetId) : IRequest;
}
