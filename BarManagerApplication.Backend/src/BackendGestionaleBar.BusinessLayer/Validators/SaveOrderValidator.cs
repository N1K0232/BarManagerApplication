using BackendGestionaleBar.Shared.Requests;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators;

public class SaveOrderValidator : AbstractValidator<SaveOrderRequest>
{
    public SaveOrderValidator()
    {
        RuleFor(o => o.OrderedQuantity)
            .NotEqual(0)
            .GreaterThan(0)
            .WithMessage("you must specify the quantity");

        RuleFor(o => o.Products)
            .NotNull()
            .WithMessage("You must specify at least one product");
    }
}