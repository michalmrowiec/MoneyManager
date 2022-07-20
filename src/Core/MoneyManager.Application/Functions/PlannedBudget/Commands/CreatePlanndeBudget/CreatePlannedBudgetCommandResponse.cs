using MoneyManager.Application.Responses;
using FluentValidation.Results;


namespace MoneyManager.Application.Functions.PlannedBudget.Commands.CreatePlanndeBudget
{
    public class CreatePlannedBudgetCommandResponse : BaseResponse
    {
        private int? PlannedBudgetId { get; set; }

        public CreatePlannedBudgetCommandResponse() : base()
        { }

        public CreatePlannedBudgetCommandResponse(ValidationResult validatorResult) : base(validatorResult)
        { }

        public CreatePlannedBudgetCommandResponse(string message) : base(message)
        { }

        public CreatePlannedBudgetCommandResponse(string message, bool succes) : base(message, succes)
        { }

        public CreatePlannedBudgetCommandResponse(int plannedBudgetId)
        {
            PlannedBudgetId = plannedBudgetId;
        }
    }
}
