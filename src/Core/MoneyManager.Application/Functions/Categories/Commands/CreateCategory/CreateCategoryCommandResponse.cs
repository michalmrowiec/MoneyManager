using FluentValidation.Results;
using MoneyManager.Application.Functions.Categories.Queries;
using MoneyManager.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandResponse : BaseResponse
    {
        public CategoryDto? CategoryDto { get; set; }
        public CreateCategoryCommandResponse() : base()
        { }

        public CreateCategoryCommandResponse(ValidationResult validationResult) : base(validationResult)
        { }

        public CreateCategoryCommandResponse(string message) : base(message)
        { }

        public CreateCategoryCommandResponse(string message, bool success) : base(message, success)
        { }

        public CreateCategoryCommandResponse(CategoryDto categoryDto)
        {
            CategoryDto = categoryDto;
        }
    }
}
