﻿using FluentValidation;

namespace MoneyManager.Application.Functions.Records
{
    public class CreateRecordCommandValidator : AbstractValidator<CreateRecordCommand>
    {
        public CreateRecordCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .NotNull()
                .MinimumLength(2)
                .WithMessage("{PropertyName} must be at least 2 characters long")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters");

            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage("{PropertyName} is required");
        }
    }
}