using FluentValidation;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudgetRecord;
using MoneyManager.Application.Functions.PlannedBudget.Queries;
using MoneyManager.Application.Functions.PlannedBudget.Queries.GetPlannedBudgetsForMonth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudget
{
    public class CreatePlannedBudgetCommandValidator : AbstractValidator<CreatePlannedBudgetCommand>
    {
        public CreatePlannedBudgetCommandValidator(IMediator mediator)
        {
            RuleFor(x => x).Custom((value, context) =>
            {
                var exist = mediator.Send(new GetPlannedBudgetsForMonthQuery(value.UserId, value.PlanForMonth.Year, value.PlanForMonth.Month)).Result;
                if (exist.Count != 0 || exist != null)
                    context.AddFailure("PlanForMonth", "The planned budget for this month for this category already exists");
            });
        }
    }
}
