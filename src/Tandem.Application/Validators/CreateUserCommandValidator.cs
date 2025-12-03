using FluentValidation;
using Tandem.Application.Commands;

namespace Tandem.Application.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Request.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(100).WithMessage("FirstName must not exceed 100 characters.");

        RuleFor(x => x.Request.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(100).WithMessage("LastName must not exceed 100 characters.");

        RuleFor(x => x.Request.MiddleName)
            .MaximumLength(100).WithMessage("MiddleName must not exceed 100 characters.")
            .When(x => x.Request.MiddleName != null);

        RuleFor(x => x.Request.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .MaximumLength(20).WithMessage("PhoneNumber must not exceed 20 characters.")
            .Matches(@"^[\d\-\(\)\s]+$").WithMessage("PhoneNumber format is invalid.");

        RuleFor(x => x.Request.EmailAddress)
            .NotEmpty().WithMessage("EmailAddress is required.")
            .EmailAddress().WithMessage("EmailAddress format is invalid.")
            .MaximumLength(255).WithMessage("EmailAddress must not exceed 255 characters.");
    }
}

