using FluentValidation.Results;

namespace MoneyManager.Application.Responses
{
    public class BaseResponse
    {
        private System.ComponentModel.DataAnnotations.ValidationResult? validatorResult;

        public ResponseStatus Status { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? ValidationErrors { get; set; }

        public BaseResponse()
        {
            ValidationErrors = new List<string>();
            Success = true;
        }
        public BaseResponse(string? message = null)
        {
            ValidationErrors = new List<string>();
            Success = true;
            Message = message;
        }

        public BaseResponse(string message, bool success)
        {
            ValidationErrors = new List<string>();
            Success = success;
            Message = message;
        }

        public BaseResponse(ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();
            Success = validationResult.Errors.Count < 0;
            foreach (var item in validationResult.Errors)
            {
                ValidationErrors.Add(item.ErrorMessage);
            }
            Status = ResponseStatus.ValidationError;
        }

        public BaseResponse(System.ComponentModel.DataAnnotations.ValidationResult validatorResult)
        {
            this.validatorResult = validatorResult;
        }
    }

    public enum ResponseStatus
    {
        Success = 0,
        NotFound = 1,
        BadQuery = 2,
        ValidationError = 3
    }
}
