using BackendGestionaleBar.Shared.Models.Requests;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators;

internal class SaveProductValidator : AbstractValidator<SaveProductRequest>
{
    public SaveProductValidator()
    {
        RuleFor(o => o.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("can't save a product with no name");

        RuleFor(o => o.CategoryName)
            .NotNull()
            .NotEmpty()
            .WithMessage("you must specify the category");

        RuleFor(o => o.ExpirationDate)
            .NotNull()
            .WithMessage("can't save a product with no expiration date");

        RuleFor(o => o.Price)
            .GreaterThan(0)
            .WithMessage("can't save a product with negative or 0 price");

        RuleFor(o => o.Quantity)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("can't save a product with negative quantity");
    }
}