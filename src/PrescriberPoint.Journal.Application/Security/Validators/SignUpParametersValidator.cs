using FluentValidation;
using PrescriberPoint.Journal.Application.Security.Commands;

namespace PrescriberPoint.Journal.Application.Security.Validators;

public class SignUpParametersValidator : AbstractValidator<SignUpUserParameters>
{
    public SignUpParametersValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .NotNull();

        RuleFor(e => e.Username)
            .NotEmpty()
            .NotNull();

        RuleFor(e => e.Password)
            .NotEmpty()
            .NotNull();
    }
}