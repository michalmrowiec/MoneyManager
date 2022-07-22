using MediatR;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord
{
    public record CreatePlannedBudgetCommand : IRequest<CreatePlannedBudgetCommandResponse>
    {
        public int Id { get; set; }
        public DateTime PlanForMonth { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
