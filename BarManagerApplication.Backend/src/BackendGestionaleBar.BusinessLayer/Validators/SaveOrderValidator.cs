using BackendGestionaleBar.Shared.Models.Responses;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators
{
    public class SaveOrderValidator : AbstractValidator<SaveOrderRequest>
    {
        public SaveOrderValidator()
        {
            RuleFor(o => o.IdUser)
                .NotEqual(Guid.Empty)
                .WithMessage("Can't save an order without the user");

            RuleFor(o => o.IdProduct)
                .NotEqual(Guid.Empty)
                .WithMessage("Can't save an order without the product");
        }
    }
}