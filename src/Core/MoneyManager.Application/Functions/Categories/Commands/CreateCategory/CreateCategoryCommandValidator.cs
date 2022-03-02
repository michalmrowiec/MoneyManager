using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MaximumLength(25)
                .WithMessage("{PropertyName} must not exceed 25 characters");
        }
    }
}
