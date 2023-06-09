using FluentValidation.Results;
using MoneyManager.Application.Responses;

namespace MoneyManager.Application.Functions.Records.Commands.CreateRecord
{
    public class CreateRecordCommandResponse : BaseResponse
    {
        public int? RecordId { get; set; }
        public CreateRecordCommandResponse() : base()
        { }

        public CreateRecordCommandResponse(ValidationResult validationResult) : base(validationResult)
        { }

        public CreateRecordCommandResponse(string message) : base(message)
        { }

        public CreateRecordCommandResponse(string message, bool succes) : base(message, succes)
        { }

        public CreateRecordCommandResponse(int recordId)
        {
            RecordId = recordId;
        }
    }
}
