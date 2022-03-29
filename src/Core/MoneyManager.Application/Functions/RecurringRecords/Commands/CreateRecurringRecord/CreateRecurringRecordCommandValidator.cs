using FluentValidation;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord
{
    public class CreateRecurringRecordCommandValidator : AbstractValidator<CreateRecurringRecordCommand>
    {
        public CreateRecurringRecordCommandValidator()
        {
            RuleFor(x => x.RepeatAfterDays)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.RepeatEveryDayOfMonth)
                .GreaterThanOrEqualTo(1);
        }
    }
}