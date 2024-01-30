using FluentValidation;
using PrescriberPoint.Journal.Domain;

public class JournalValidator: AbstractValidator<Journal> {
    public JournalValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .GreaterThan(0)
            .When(e => e.Id == 0);

        RuleFor(x => x.Patient)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);

        RuleFor(x => x.Note)
            .NotNull()
            .MaximumLength(200);
    }
}