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

        RuleFor(o => o.ProductIds)
            .NotNull()
            .WithMessage("Can't save an order without the products");
    }
}