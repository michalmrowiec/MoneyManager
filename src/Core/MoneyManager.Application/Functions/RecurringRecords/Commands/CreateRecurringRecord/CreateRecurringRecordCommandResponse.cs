using FluentValidation.Results;
using MoneyManager.Application.Responses;

namespace MoneyManager.Application.Functions.RecurringRecords.Commands.CreateRecurringRecord
{
    public class CreateRecurringRecordCommandResponse : BaseResponse
    {
        private int? RecurringRecordId { get; set; }

        public CreateRecurringRecordCommandResponse() : base()
        { }

        public CreateRecurringRecordCommandResponse(ValidationResult validatorResult) : base(validatorResult)
        { }

        public CreateRecurringRecordCommandResponse(string message) : base(message)
        { }

        public CreateRecurringRecordCommandResponse(string message, bool succes) : base(message, succes)
        { }

        public CreateRecurringRecordCommandResponse(int recurringRecordId)
        {
            RecurringRecordId = recurringRecordId;
        }
    }
}