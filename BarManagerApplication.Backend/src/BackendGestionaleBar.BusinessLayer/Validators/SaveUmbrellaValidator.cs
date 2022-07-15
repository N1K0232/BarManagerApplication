using BackendGestionaleBar.Shared.Requests;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators;

internal class SaveUmbrellaValidator : AbstractValidator<SaveUmbrellaRequest>
{
    public SaveUmbrellaValidator()
    {
        RuleFor(u => u.Number)
            .GreaterThanOrEqualTo(100)
            .WithMessage("add a valid umbrella number");

        RuleFor(u => u.Letter)
            .NotNull()
            .NotEmpty()
            .MaximumLength(1)
            .WithMessage("insert a valid letter");
    }
}